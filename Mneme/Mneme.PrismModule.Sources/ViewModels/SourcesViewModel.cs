using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MaterialDesignThemes.Wpf;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Model.Sources;
using Mneme.PrismModule.Sources.Views;
using Mneme.Sources;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
namespace Mneme.PrismModule.Sources.ViewModels
{
	internal class SourcesViewModel : BindableBase, INavigationAware
	{
		private ObservableCollection<SourcePreview> sources;
		private readonly ISnackbarMessageQueue snackbarMessageQueue;
		private readonly IDialogService dialogService;
		private readonly SourcesManager manager;
		private CancellationTokenSource cts;
		private string searchedPhrase;
		private List<SourcePreview> allSourcesPreview;
		private SourcePreview selectedSource;
		public SourcePreview SelectedSource
		{
			get => selectedSource;
			set => SetProperty(ref selectedSource, value);
		}

		private bool isLoading;
		public bool IsLoading
		{
			get => isLoading;
			set => SetProperty(ref isLoading, value);
		}

		public ObservableCollection<SourcePreview> Sources
		{
			get => sources;
			set => SetProperty(ref sources, value);
		}
		
		private bool sourcesListEmpty;
		public bool SourcesListEmpty
		{
			get => Sources.Count == 0 && !isLoading;
		}

		public SourcesViewModel(ISnackbarMessageQueue snackbarMessageQueue, IDialogService dialogService, SourcesManager manager)
		{
			Sources = [];
			allSourcesPreview = [];
			this.snackbarMessageQueue = snackbarMessageQueue;
			this.dialogService = dialogService;
			this.manager = manager;
			IgnoreSourceCommand = new DelegateCommand<SourcePreview>(IgnoreSource, (x) => x?.IsActive ?? false);
			ActivateSourceCommand = new DelegateCommand<SourcePreview>(ActivateSource, (x) => !x?.IsActive ?? false);
			DeleteSourceCommand = new DelegateCommand<SourcePreview>(DeleteSource);
			EditSourceCommand = new DelegateCommand<SourcePreview>(EditSource, (x) => x?.TypeOfSource == MnemeSource.Type);
			ShowDialogCreateSourceCommand = new DelegateCommand(CreateSource);
			IsLoading = true;
		}

		private void EditSource(SourcePreview source)
		{
			var param = new DialogParameters
			{
				{ "source", source}
			};
			dialogService.ShowDialog(nameof(SourceCreationView), param, result =>
			{
				if (result.Result == ButtonResult.OK)
				{
					var editedSource = result.Parameters.GetValue<SourcePreview>("source");
					Sources.Remove(source);
					Sources.Add(editedSource);
					SelectedSource = editedSource;
				}
			});
		}

		private async void DeleteSource(SourcePreview source)
		{
			if (await manager.DeleteSource(source)) 
			{
				Sources.Remove(source);
				RaisePropertyChanged(nameof(SourcesListEmpty));
			}
			else
				snackbarMessageQueue.Enqueue("Could not delete source. Some related data to this source still exists.");
		}

		private async void IgnoreSource(SourcePreview source)
		{
			var updatedSource = await manager.IgnoreSource(source);
			Sources.Remove(source);
			Sources.Add(updatedSource);
			SelectedSource = updatedSource;
		}

		private async void ActivateSource(SourcePreview source)
		{
			var updatedSource = await manager.ActivateSource(source);
			Sources.Remove(source);
			Sources.Add(updatedSource);
			SelectedSource = updatedSource;
		}

		public DelegateCommand<SourcePreview> IgnoreSourceCommand { get; set; }
		public DelegateCommand<SourcePreview> ActivateSourceCommand { get; set; }
		public DelegateCommand<SourcePreview> EditSourceCommand { get; set; }
		public DelegateCommand<SourcePreview> DeleteSourceCommand { get; set; }
		public DelegateCommand ShowDialogCreateSourceCommand { get; }

		private void CreateSource()
		{
			dialogService.ShowDialog(nameof(SourceCreationView), null, result =>
			{
				if (result.Result == ButtonResult.OK)
				{
					var source = result.Parameters.GetValue<MnemeSource>("source");
					var preview = SourcePreview.CreateFromSource(source);
					Sources.Add(preview);
					allSourcesPreview.Add(preview);
					RaisePropertyChanged(nameof(SourcesListEmpty));
				}
			});
		}

		public string SearchedPhrase
		{
			get => searchedPhrase;
			set
			{
				SetProperty(ref searchedPhrase, value);
				if (searchedPhrase.Length > 2)
				{
					Sources = new ObservableCollection<SourcePreview>(allSourcesPreview.Where(x => x.Title.ToLower().Contains(searchedPhrase.ToLower()) || x.TypeOfSource.ToLower() == searchedPhrase.ToLower()));
				}
				else if (Sources.Count != allSourcesPreview.Count)
				{
					Sources = new ObservableCollection<SourcePreview>(allSourcesPreview);
				}
			}
		}

		public async void OnNavigatedTo(NavigationContext navigationContext)
		{
			using (cts = new CancellationTokenSource())
			{
				IsLoading = true;
				await Task.Run(async () =>
				{
					IEnumerable<SourcePreview> sourcesPreview = [];
					try
					{
						sourcesPreview = await manager.GetSourcesPreviewAsync(cts.Token);
					}
					catch (TaskCanceledException) { }
					Application.Current.Dispatcher.Invoke(() =>
					{
						Sources.Clear();
						foreach (var source in sourcesPreview)
						{
							Sources.Add(source);
						}
						allSourcesPreview = new List<SourcePreview>(Sources);
						IsLoading = false;
						RaisePropertyChanged(nameof(SourcesListEmpty));
					});
				});
			}
		}

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return true;
		}

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{
			try
			{
				cts?.Cancel();
			}
			catch (System.ObjectDisposedException) { }
		}
	}
}
