using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MaterialDesignThemes.Wpf;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Model;
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
		private ObservableCollection<Source> sources;
		private readonly ISnackbarMessageQueue snackbarMessageQueue;
		private readonly IDialogService dialogService;
		private readonly SourcesManager manager;
		private CancellationTokenSource cts;
		private string searchedPhrase;
		private List<Source> allSourcesPreview;
		private Source selectedSource;
		public Source SelectedSource
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

		public ObservableCollection<Source> Sources
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
			IgnoreSourceCommand = new DelegateCommand<Source>(IgnoreSource, (x) => x?.Active ?? false);
			ActivateSourceCommand = new DelegateCommand<Source>(ActivateSource, (x) => !x?.Active ?? false);
			DeleteSourceCommand = new DelegateCommand<Source>(DeleteSource);
			EditSourceCommand = new DelegateCommand<Source>(EditSource, (x) => x?.TextType == MnemeSource.Type);
			ShowDialogCreateSourceCommand = new DelegateCommand(CreateSource);
			IsLoading = true;
		}

		private void EditSource(Source source)
		{
			var param = new DialogParameters
			{
				{ "source", source}
			};
			dialogService.ShowDialog(nameof(SourceCreationView), param, result =>
			{
				if (result.Result == ButtonResult.OK)
				{
					var editedSource = result.Parameters.GetValue<Source>("source");
					Sources.Remove(source);
					Sources.Add(editedSource);
					SelectedSource = editedSource;
				}
			});
		}

		private async void DeleteSource(Source source)
		{
			if (await manager.DeleteSource(source))
			{
				Sources.Remove(source);
				RaisePropertyChanged(nameof(SourcesListEmpty));
			}
			else
				snackbarMessageQueue.Enqueue("Could not delete source. Some related data to this source still exists.");
		}

		private async void IgnoreSource(Source source)
		{
			var updatedSource = await manager.IgnoreSource(source);
			Sources.Remove(source);
			Sources.Add(updatedSource);
			SelectedSource = updatedSource;
		}

		private async void ActivateSource(Source source)
		{
			var updatedSource = await manager.ActivateSource(source);
			Sources.Remove(source);
			Sources.Add(updatedSource);
			SelectedSource = updatedSource;
		}

		public DelegateCommand<Source> IgnoreSourceCommand { get; set; }
		public DelegateCommand<Source> ActivateSourceCommand { get; set; }
		public DelegateCommand<Source> EditSourceCommand { get; set; }
		public DelegateCommand<Source> DeleteSourceCommand { get; set; }
		public DelegateCommand ShowDialogCreateSourceCommand { get; }

		private void CreateSource()
		{
			dialogService.ShowDialog(nameof(SourceCreationView), null, result =>
			{
				if (result.Result == ButtonResult.OK)
				{
					var source = result.Parameters.GetValue<MnemeSource>("source");
					Sources.Add(source);
					allSourcesPreview.Add(source);
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
					Sources = new ObservableCollection<Source>(allSourcesPreview.Where(x => x.Title.ToLower().Contains(searchedPhrase.ToLower()) || x.TextType.ToLower() == searchedPhrase.ToLower()));
				}
				else if (Sources.Count != allSourcesPreview.Count)
				{
					Sources = new ObservableCollection<Source>(allSourcesPreview);
				}
			}
		}

		public async void OnNavigatedTo(NavigationContext navigationContext)
		{
			using (cts = new CancellationTokenSource())
			{
				IsLoading = true;
				var getSourcesTask = manager.GetSourcesPreviewAsync(cts.Token);
				var completedTask = await Task.WhenAny(getSourcesTask, Task.Delay(Timeout.Infinite, cts.Token));

				if (completedTask == getSourcesTask)
				{
					Sources.Clear();
					Sources.AddRange(getSourcesTask.Result);
					allSourcesPreview = new List<Source>(Sources);//todo
					IsLoading = false;
					RaisePropertyChanged(nameof(SourcesListEmpty));
				}
			}
			cts = null;
		}

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return true;
		}

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{
			cts?.Cancel();
		}
	}
}
