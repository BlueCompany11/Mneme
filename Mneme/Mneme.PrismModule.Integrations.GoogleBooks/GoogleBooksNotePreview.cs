using Mneme.Integrations.GoogleBooks.Contract;

namespace Mneme.PrismModule.Integrations.GoogleBooks
{
	public class GoogleBooksNotePreview
	{
		public string Title { get; set; }
		public string SourceType { get; set; }
		public string LastEdited { get; set; }
		public string CreationDate { get; set; }
		public string Link { get; set; }
		public string Type { get; set; }
		public string NoteText { get; set; }

		public static GoogleBooksNotePreview CreateFromNote(GoogleBooksNote note)
		{
			return new GoogleBooksNotePreview()
			{
				CreationDate = note.CreationTime.ToString(),
				LastEdited = note.CreationTime.ToString(),
				Link = note.Path,
				SourceType = "Google Books",
				Title = note.Title,
				NoteText = note.Content,
				Type = note.NoteType
			};
		}
	}
}
