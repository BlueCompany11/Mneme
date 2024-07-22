using Mneme.Integrations.Mneme.Contract;
using Mneme.Model;
using Mneme.Notes;
using Mneme.PrismModule.Integration.Facade;
using Mneme.PrismModule.Notes.Views;
using Mneme.Views.Base;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mneme.PrismModule.Notes.ViewModels;

public class NotesViewModel : SearchableViewModel<Note>, INavigationAware
{
	private readonly IRegionManager regionManager;
	private readonly NotesUtility utilty;
	private readonly NoteToPreviewNavigator navigator;
	private bool isLoading;
	private Note selectedNotePreview;
	private CancellationTokenSource cts;
	private string deleteNoteToolTip;

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

	public DelegateCommand OpenNewNoteViewCommand { get; set; }
	public DelegateCommand<Note> DeleteNoteCommand { get; set; }
	public NotesViewModel(IRegionManager regionManager, NotesUtility utilty, NoteToPreviewNavigator navigator) : base()
	{
		this.regionManager = regionManager;
		this.utilty = utilty;
		this.navigator = navigator;
		OpenNewNoteViewCommand = new DelegateCommand(OpenNewNoteView);
		DeleteNoteCommand = new DelegateCommand<Note>(DeleteNote, (p) => p?.GetType() == typeof(MnemeNote));
	}

	private void OpenNewNoteView() => regionManager.RequestNavigate(RegionNames.NoteRegion, nameof(NewMnemeNoteView));

	private async void DeleteNote(Note preview)
	{
		await utilty.DeleteNote(preview);
		_ = AllItems.Remove(preview);
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
				Task<IReadOnlyList<Note>> getNotesTask = utilty.GetNotes(cts.Token);
				Task completedTask = await Task.WhenAny(getNotesTask, Task.Delay(Timeout.Infinite, cts.Token));

				if (completedTask == getNotesTask)
				{
					IReadOnlyList<Note> notes = getNotesTask.Result;
					if (notes.Count != AllItems.Count)
					{
						AllItems.Clear();
						_ = AllItems.AddRange(notes.OrderBy(x => x.CreationTime));
					}
				}
			}
			IsLoading = false;
			cts = null;
		}

		void ShowNewMnemeNote(NavigationContext navigationContext)
		{
			var note = (MnemeNote)navigationContext.Parameters["note"];
			AllItems.Insert(0, note);
			RaisePropertyChanged(nameof(AllItems));
			SelectedNotePreview = note;
		}
	}
	public bool IsNavigationTarget(NavigationContext navigationContext) => true;
	public void OnNavigatedFrom(NavigationContext navigationContext)
	{
		if (navigationContext.Uri.OriginalString == nameof(NewMnemeNoteView))
			return;
		if (AllItems.Count != 0)
			cts?.Cancel();
	}

	protected override Func<Note, bool> SearchCondition() => x => x.Title.ToLower().Contains(SearchedPhrase.ToLower()) || x.Content.ToLower().Contains(SearchedPhrase.ToLower());
}
