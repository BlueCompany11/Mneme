using Mneme.Model;
using Mneme.Notes;
using Mneme.PrismModule.Notes.Views;
using Mneme.Views.Base;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace Mneme.PrismModule.Notes.ViewModels;

public class NewMnemeNoteViewModel : BindableBase, INavigationAware
{
	private readonly IRegionManager regionManager;
	private readonly MnemeNotesProxy mnemeNotesProxy;

	public ObservableCollection<Source> SourcesPreviews { get; set; }

	private Source selectedSourcePreview;
	public Source SelectedSourcePreview
	{
		get => selectedSourcePreview;
		set => SetProperty(ref selectedSourcePreview, value);
	}

	private string title;
	public string Title
	{
		get => title;
		set => SetProperty(ref title, value);
	}

	private string note;
	public string Note
	{
		get => note;
		set => SetProperty(ref note, value);
	}

	private string noteDetails;
	public string NoteDetails
	{
		get => noteDetails;
		set => SetProperty(ref noteDetails, value);
	}
	public DelegateCommand CreateNoteCommand { get; set; }
	public NewMnemeNoteViewModel(IRegionManager regionManager, MnemeNotesProxy manager)
	{
		this.regionManager = regionManager;
		this.mnemeNotesProxy = manager;
		SourcesPreviews = [];
		CreateNoteCommand = new DelegateCommand(CreateNote, CanCreateNote())
			.ObservesProperty(() => SelectedSourcePreview)
			.ObservesProperty(() => Title);
	}

	private Func<bool> CanCreateNote() => () => SelectedSourcePreview is not null && !string.IsNullOrEmpty(Title);

	private async void CreateNote()
	{
		Mneme.Integrations.Mneme.Contract.MnemeNote note = await mnemeNotesProxy.SaveMnemeNote(SelectedSourcePreview, Note, Title, NoteDetails, default);
		var para = new NavigationParameters() {
				{ "note", note }
			};
		regionManager.RequestNavigate(RegionNames.ContentRegion, nameof(NotesView), para);
	}

	public bool IsNavigationTarget(NavigationContext navigationContext) => true;

	public void OnNavigatedFrom(NavigationContext navigationContext)
	{

	}

	public async void OnNavigatedTo(NavigationContext navigationContext)
	{
		await Task.Run(async () =>
		{
			var sources = await mnemeNotesProxy.GetMnemeSources(default).ConfigureAwait(false);
			Application.Current.Dispatcher.Invoke(() =>
			{
				SourcesPreviews.Clear();
				_ = SourcesPreviews.AddRange(sources);
			});
		});
	}
}
