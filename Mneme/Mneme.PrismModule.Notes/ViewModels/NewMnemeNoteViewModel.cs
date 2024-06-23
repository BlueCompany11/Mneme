using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Mneme.Core.Interfaces;
using Mneme.Integrations.Contracts;
using Mneme.Integrations.Mneme;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Model.Sources;
using Mneme.Notes;
using Mneme.PrismModule.Notes.Views;
using Mneme.Views.Base;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Mneme.PrismModule.Notes.ViewModels
{
	public class NewMnemeNoteViewModel : BindableBase, INavigationAware
	{
		private readonly IRegionManager regionManager;
		private readonly MnemeNotesCreator manager;
		private ObservableCollection<SourcePreview> sourcesPreview;
		public ObservableCollection<SourcePreview> SourcesPreviews 
		{
			get => sourcesPreview;
			set => SetProperty(ref sourcesPreview, value);
		}

		private SourcePreview selectedSourcePreview;
		public SourcePreview SelectedSourcePreview
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
		public NewMnemeNoteViewModel(IRegionManager regionManager, MnemeNotesCreator manager)
		{
			this.regionManager = regionManager;
			this.manager = manager;
			SourcesPreviews = [];
			CreateNoteCommand = new DelegateCommand(CreateNote, CanCreateNote())
				.ObservesProperty(() => SelectedSourcePreview)
				.ObservesProperty(() => Title);
		}

		private Func<bool> CanCreateNote()
		{
			return () => SelectedSourcePreview is not null && !string.IsNullOrEmpty(Title);
		}

		private async void CreateNote()
		{
			var note = await manager.SaveMnemeNote(SelectedSourcePreview, Note, Title, NoteDetails, default);
			var para = new NavigationParameters() {
				{ "note", note }
			};
			regionManager.RequestNavigate(RegionNames.ContentRegion, nameof(NotesView), para);
		}

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return true;
		}

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{

		}

		public async void OnNavigatedTo(NavigationContext navigationContext)
		{
			await Task.Run(async () =>
			{
				IEnumerable<SourcePreview> sourcesPreview = [];
				sourcesPreview = await manager.GetSourcesPreviews(default);
				Application.Current.Dispatcher.Invoke(() =>
				{
					SourcesPreviews.Clear();
					SourcesPreviews.AddRange(sourcesPreview);
				});
			});
		}
	}
}
