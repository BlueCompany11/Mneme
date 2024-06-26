using Mneme.Model.Interfaces;
using Mneme.Model.Preelaborations;

namespace Mneme.Integrations.GoogleBooks.Contract
{
	public class GoogleBooksNote : Note
	{
		public required string NoteType { get; init; }
		public int SourceId { get; init; }
		public required GoogleBooksSource? Source { get; set; }

		public override INoteTest Accept(INoteTestVisitor visitor)
		{
			return visitor is INoteTestVisitor<GoogleBooksNote> v
				? v.GetTestNote(this)
				: throw new Exception("Wrong interface for " + visitor.GetType());
		}
	}
}
