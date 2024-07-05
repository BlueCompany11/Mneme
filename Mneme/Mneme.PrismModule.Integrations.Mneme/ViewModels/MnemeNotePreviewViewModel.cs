using System;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Model.Notes;
using Mneme.PrismModule.Integrations.Base;
using Prism.Regions;

namespace Mneme.PrismModule.Integrations.Mneme.ViewModels
{
	public class MnemeNotePreviewViewModel : AbstractNotePreviewViewModel
	{
		private MnemeNotePreview notePreview;
		private string title;
		public string Title
		{
			get => title;
			set => SetProperty(ref title, value);
		}

		private string source;
		public string Source
		{
			get => source;
			set => SetProperty(ref source, value);
		}

		private string sourceDetails;
		public string SourceDetails
		{
			get => sourceDetails;
			set => SetProperty(ref sourceDetails, value);
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
		private DateTime creationDate;
		public DateTime CreationDate
		{
			get => creationDate;
			set => SetProperty(ref creationDate, value);
		}
		protected override Note BaseNote { get; set; }

		public MnemeNotePreviewViewModel(IRegionManager regionManager) : base(regionManager) {}
		protected override void LoadNote()
		{
			notePreview = MnemeNotePreview.CreateFromNote((MnemeNote)BaseNote);
			NotePreviewUpdate();
		}

		private void NotePreviewUpdate()
		{
			Title = notePreview.Title;
			Note = notePreview.NoteText;
			NoteDetails = notePreview.NoteDetails;
			Source = notePreview.Source;
			SourceDetails = notePreview.SourceDetails;
			CreationDate = notePreview.CreationDate;
		}
	}
}
