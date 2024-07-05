using Mneme.Model.Notes;

namespace Mneme.Integrations.GoogleBooks.Contract
{
	public class GoogleBooksNote : Note
	{
		public required string NoteType { get; init; }
		public int SourceId { get; init; }
		public required GoogleBooksSource? Source { get; set; }
	}
}
