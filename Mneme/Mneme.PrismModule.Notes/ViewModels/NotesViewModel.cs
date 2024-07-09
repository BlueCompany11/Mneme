using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Model;
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
		private Note selectedNotePreview;
		private List<Note> Notes { get; set; }
		private ObservableCollection<Note> notesPreview;
		private CancellationTokenSource cts;
		private string searchedPhrase;
		private string deleteNoteToolTip;
		private List<Note> cachedNotesPreview;
		public ObservableCollection<Note> NotesPreview
		{
			get => notesPreview;
			set => SetProperty(ref notesPreview, value);
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
					NotesPreview = new ObservableCollection<Note>(cachedNotesPreview.Where(x => x.Title.ToLower().Contains(searchedPhrase.ToLower()) || x.Content.ToLower().Contains(searchedPhrase.ToLower())));
				}
				else if (NotesPreview.Count != cachedNotesPreview.Count)
				{
					NotesPreview = new ObservableCollection<Note>(cachedNotesPreview);
				}
			}

		}
		public Note SelectedNotePreview
		{
			get => selectedNotePreview;
			set
			{
				if (selectedNotePreview != value)
				{
					_ = SetProperty(ref selectedNotePreview, value);
					DeleteNoteToolTip = SelectedNotePreview?.GetType() == typeof(MnemeNote) ? "Delete note" : "Only notes created by the user can be deleted";
					Navigate();
				}
			}
		}
		private static readonly object _syncLock = new();

		public DelegateCommand OpenNewNoteViewCommand { get; set; }
		public DelegateCommand<Note> DeleteNoteCommand { get; set; }
		public NotesViewModel(IRegionManager regionManager, NotesUtility utilty, NoteToPreviewNavigator navigator)
		{
			NotesPreview = [];
			Notes = [];
			cachedNotesPreview = [];
			this.regionManager = regionManager;
			this.utilty = utilty;
			this.navigator = navigator;
			BindingOperations.EnableCollectionSynchronization(NotesPreview, _syncLock);
			OpenNewNoteViewCommand = new DelegateCommand(OpenNewNoteView);
			DeleteNoteCommand = new DelegateCommand<Note>(DeleteNote, (p) => p?.GetType() == typeof(MnemeNote));
		}

		private async Task GetNotes(CancellationToken ct)
		{
			Notes = new List<Note>(await utilty.GetNotes(ct));
		}

		private void NotesProvider_NotesUpdated()
		{
			Notes = Notes.OrderByDescending(x => x.CreationTime).ToList();
			lock (_syncLock)
			{
				NotesPreview.Clear();
				foreach (var item in Notes)
				{
					NotesPreview.Add(item);
				}
				cachedNotesPreview = new List<Note>(NotesPreview);
			}
		}

		private void OpenNewNoteView()
		{
			regionManager.RequestNavigate(RegionNames.NoteRegion, nameof(NewMnemeNoteView));
		}

		private async void DeleteNote(Note preview)
		{
			await utilty.DeleteNote(preview);
			NotesPreview.Remove(preview);
		}

		private void Navigate()
		{
			var para = new NavigationParameters() {
				{ "note", SelectedNotePreview }
			};
			navigator.NavigateToPreview(SelectedNotePreview, para, RegionNames.NoteRegion);

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
							Notes = new List<Note>(await utilty.GetNotes(cts.Token));
							Notes = Notes.OrderByDescending(x => x.CreationTime).ToList();
							cachedNotesPreview = new List<Note>(NotesPreview);
						}
						catch (TaskCanceledException) { }
						Application.Current.Dispatcher.Invoke(() =>
						{
							lock (_syncLock)
							{
								NotesPreview.Clear();
								NotesPreview.AddRange(Notes);
							}
							IsLoading = false;
						});
					});
				}
			}

			void ShowNewMnemeNote(NavigationContext navigationContext)
			{
				var note = (MnemeNote)navigationContext.Parameters["note"];
				NotesPreview.Insert(0, note);
				SelectedNotePreview = note;
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
