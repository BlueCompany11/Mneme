using Mneme.Model.Interfaces;
using Mneme.Model.Preelaborations;

namespace Mneme.Integrations.GoogleBooks.Contract
{
	public class GoogleBooksPreelaboration : Preelaboration
	{
		public required string NoteType { get; init; }
		public int SourceId { get; init; }
		public required GoogleBooksSource? Source { get; set; }

		public override INoteTest Accept(INoteTestVisitor visitor)
		{
			return visitor is INoteTestVisitor<GoogleBooksPreelaboration> v
				? v.GetTestNote(this)
				: throw new Exception("Wrong interface for " + visitor.GetType());
		}
	}
}
