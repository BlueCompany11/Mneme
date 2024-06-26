using Mneme.Integrations.GoogleBooks.Contract;
using Mneme.Model.Notes;
using Mneme.PrismModule.Integrations.Base;
using Mneme.Views.Base;
using Prism.Regions;

namespace Mneme.PrismModule.Integrations.GoogleBooks.ViewModels
{
	public class GoogleBooksNotePreviewViewModel : AbstractNotePreviewViewModel
	{
		private GoogleBooksNotePreview notePreview;
		private string title;
		public string Title
		{
			get => title;
			set => SetProperty(ref title, value);
		}
		private string sourceType;
		public string SourceType
		{
			get => sourceType;
			set => SetProperty(ref sourceType, value);
		}

		private string lastEdited;
		public string LastEdited
		{
			get => lastEdited;
			set => SetProperty(ref lastEdited, value);
		}

		private string creationDate;
		public string CreationDate
		{
			get => creationDate;
			set => SetProperty(ref creationDate, value);
		}

		private string link;
		public string Link
		{
			get => link;
			set => SetProperty(ref link, value);
		}
		private string type;
		public string Type
		{
			get => type;
			set => SetProperty(ref type, value);
		}
		private string noteText;
		public string NoteText
		{
			get => noteText;
			set => SetProperty(ref noteText, value);
		}

		protected override Note Preelaboration { get; set; }

		public GoogleBooksNotePreviewViewModel(IRegionManager regionManager) : base(regionManager) {}

		private void GoogleBooksNotePreviewUpdate()
		{
			Title = notePreview.Title;
			SourceType = notePreview.SourceType;
			CreationDate = notePreview.CreationDate;
			Link = notePreview.Link;
			LastEdited = notePreview.LastEdited;
			Type = notePreview.Type;
			NoteText = notePreview.NoteText;
		}

		protected override void LoadNote()
		{
			notePreview = GoogleBooksNotePreview.CreateFromNote((GoogleBooksNote)Preelaboration);
			GoogleBooksNotePreviewUpdate();
		}
	}
}
