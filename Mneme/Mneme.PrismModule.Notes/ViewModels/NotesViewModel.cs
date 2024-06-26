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
using Mneme.Model.Notes;
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
		private NotePreview selectedNotePreview;
		private List<Note> Notes { get; set; }
		private Note SelectedNote { get; set; }
		private ObservableCollection<NotePreview> notesPreview;
		private CancellationTokenSource cts;
		private string searchedPhrase;
		private string deleteNoteToolTip;
		private List<NotePreview> cachedNotesPreview;
		public ObservableCollection<NotePreview> NotesPreview
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
					NotesPreview = new ObservableCollection<NotePreview>(cachedNotesPreview.Where(x => x.Title.ToLower().Contains(searchedPhrase.ToLower()) || x.Note.ToLower().Contains(searchedPhrase.ToLower())));
				}
				else if (NotesPreview.Count != cachedNotesPreview.Count)
				{
					NotesPreview = new ObservableCollection<NotePreview>(cachedNotesPreview);
				}
			}

		}
		public NotePreview SelectedNotePreview
		{
			get => selectedNotePreview;
			set
			{
				if (selectedNotePreview != value)
				{
					_ = SetProperty(ref selectedNotePreview, value);
					SelectedNote = selectedNotePreview?.BaseNote;
					DeleteNoteToolTip = SelectedNote?.GetType() == typeof(MnemeNote) ? "Delete note" : "Only notes created by the user can be deleted";
					Navigate();
				}
			}
		}
		private static readonly object _syncLock = new();

		public DelegateCommand OpenNewNoteViewCommand { get; set; }
		public DelegateCommand<NotePreview> DeleteNoteCommand { get; set; }
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
			DeleteNoteCommand = new DelegateCommand<NotePreview>(DeleteNote, (p) => p?.BaseNote.GetType() == typeof(MnemeNote));
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
					NotesPreview.Add(NotePreview.CreateFromNote(item));
				}
				cachedNotesPreview = new List<NotePreview>(NotesPreview);
			}
		}

		private void OpenNewNoteView()
		{
			regionManager.RequestNavigate(RegionNames.NoteRegion, nameof(NewMnemeNoteView));
		}

		private async void DeleteNote(NotePreview preview)
		{
			await utilty.DeleteNote(preview);
			NotesPreview.Remove(preview);
		}

		private void Navigate()
		{
			var para = new NavigationParameters() {
				{ "note", SelectedNote }
			};
			navigator.NavigateToPreview(SelectedNote, para, RegionNames.NoteRegion);
			
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
							await GetNotes(cts.Token);
						}
						catch (TaskCanceledException) { }
						Application.Current.Dispatcher.Invoke(() =>
						{
							NotesProvider_NotesUpdated();
							IsLoading = false;
						});
					});
				}
			}

			void ShowNewMnemeNote(NavigationContext navigationContext)
			{
				var note = (MnemeNote)navigationContext.Parameters["note"];
				var preview = NotePreview.CreateFromNote(note);
				NotesPreview.Insert(0, preview);
				SelectedNote = note;
				SelectedNotePreview = preview;
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
