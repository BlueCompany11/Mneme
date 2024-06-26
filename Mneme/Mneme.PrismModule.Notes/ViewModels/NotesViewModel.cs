using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Mneme.Integrations.GoogleBooks.Contract;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Integrations.Pluralsight.Contract;
using Mneme.Model.Preelaborations;
using Mneme.Notes;
using Mneme.PrismModule.Integration.Facade;
using Mneme.PrismModule.Notes.Views;
using Mneme.Views.Base;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Mneme.PrismModule.Notes.ViewModels
{
	public class NotesViewModel : BindableBase, INavigationAware
	{
		private readonly IRegionManager regionManager;
		private readonly NotesUtility utilty;
		private readonly NoteToPreviewNavigator navigator;
		private bool isLoading;
		private PreelaborationPreview selectedPreelaborationPreview;
		private List<Preelaboration> Preelaborations { get; set; }
		private Preelaboration SelectedPreelaboration { get; set; }
		private ObservableCollection<PreelaborationPreview> preelaborationsPreview;
		private CancellationTokenSource cts;
		private string searchedPhrase;
		private string deleteNoteToolTip;
		private List<PreelaborationPreview> cachedPreelaborationsPreview;
		public ObservableCollection<PreelaborationPreview> PreelaborationsPreview
		{
			get => preelaborationsPreview;
			set => SetProperty(ref preelaborationsPreview, value);
		}

		public bool IsLoading
		{
			get => isLoading;
			set => SetProperty(ref isLoading, value);
		}

		public string DeleteNoteToolTip
		{
			get => deleteNoteToolTip;
			set => SetProperty(ref deleteNoteToolTip, value);
		}

		public string SearchedPhrase
		{
			get => searchedPhrase;
			set
			{
				SetProperty(ref searchedPhrase, value);
				if (searchedPhrase.Length > 2)
				{
					PreelaborationsPreview = new ObservableCollection<PreelaborationPreview>(cachedPreelaborationsPreview.Where(x => x.Title.ToLower().Contains(searchedPhrase.ToLower()) || x.Note.ToLower().Contains(searchedPhrase.ToLower())));
				}
				else if (PreelaborationsPreview.Count != cachedPreelaborationsPreview.Count)
				{
					PreelaborationsPreview = new ObservableCollection<PreelaborationPreview>(cachedPreelaborationsPreview);
				}
			}

		}
		public PreelaborationPreview SelectedPreelaborationPreview
		{
			get => selectedPreelaborationPreview;
			set
			{
				if (selectedPreelaborationPreview != value)
				{
					_ = SetProperty(ref selectedPreelaborationPreview, value);
					SelectedPreelaboration = selectedPreelaborationPreview?.Preelaboration;
					DeleteNoteToolTip = SelectedPreelaboration?.GetType() == typeof(MnemePreelaboration) ? "Delete note" : "Only notes created by the user can be deleted";
					Navigate();
				}
			}
		}
		private static readonly object _syncLock = new();

		public DelegateCommand OpenNewNoteViewCommand { get; set; }
		public DelegateCommand<PreelaborationPreview> DeleteNoteCommand { get; set; }
		public NotesViewModel(IRegionManager regionManager, NotesUtility utilty, NoteToPreviewNavigator navigator)
		{
			PreelaborationsPreview = [];
			Preelaborations = [];
			cachedPreelaborationsPreview = [];
			this.regionManager = regionManager;
			this.utilty = utilty;
			this.navigator = navigator;
			BindingOperations.EnableCollectionSynchronization(PreelaborationsPreview, _syncLock);
			OpenNewNoteViewCommand = new DelegateCommand(OpenNewNoteView);
			DeleteNoteCommand = new DelegateCommand<PreelaborationPreview>(DeleteNote, (p) => p?.Preelaboration.GetType() == typeof(MnemePreelaboration));
		}

		private async Task GetPreelaborations(CancellationToken ct)
		{
			Preelaborations = new List<Preelaboration>(await utilty.GetNotes(ct));
		}

		private void PreelaborationsProvider_PreelaborationsUpdated()
		{
			Preelaborations = Preelaborations.OrderByDescending(x => x.CreationTime).ToList();
			lock (_syncLock)
			{
				PreelaborationsPreview.Clear();
				foreach (var item in Preelaborations)
				{
					PreelaborationsPreview.Add(PreelaborationPreview.CreateFromNote(item));
				}
				cachedPreelaborationsPreview = new List<PreelaborationPreview>(PreelaborationsPreview);
			}
		}

		private void OpenNewNoteView()
		{
			regionManager.RequestNavigate(RegionNames.PreelaborationRegion, nameof(NewMnemeNoteView));
		}

		private async void DeleteNote(PreelaborationPreview preview)
		{
			await utilty.DeleteNote(preview);
			PreelaborationsPreview.Remove(preview);
		}

		private void Navigate()
		{
			var para = new NavigationParameters() {
				{ "pre", SelectedPreelaboration }
			};
			navigator.NavigateToPreview(SelectedPreelaboration, para, RegionNames.PreelaborationRegion);
			
		}
		public async void OnNavigatedTo(NavigationContext navigationContext)
		{
			if (navigationContext.Parameters.ContainsKey("note"))
			{
				ShowNewMnemeNote(navigationContext);
			}
			else
			{
				using (cts = new CancellationTokenSource())
				{
					IsLoading = true;
					await Task.Run(async () =>
					{
						try
						{
							await GetPreelaborations(cts.Token);
						}
						catch (TaskCanceledException) { }
						Application.Current.Dispatcher.Invoke(() =>
						{
							PreelaborationsProvider_PreelaborationsUpdated();
							IsLoading = false;
						});
					});
				}
			}

			void ShowNewMnemeNote(NavigationContext navigationContext)
			{
				var note = (MnemePreelaboration)navigationContext.Parameters["note"];
				var preview = PreelaborationPreview.CreateFromNote(note);
				PreelaborationsPreview.Insert(0, preview);
				SelectedPreelaboration = note;
				SelectedPreelaborationPreview = preview;
			}
		}
		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return true;
		}
		public void OnNavigatedFrom(NavigationContext navigationContext)
		{
			if (navigationContext.Uri.OriginalString == nameof(NewMnemeNoteView))
				return;
			try
			{
				cts?.Cancel();
			}
			catch (System.ObjectDisposedException) { }
		}
	}
}
