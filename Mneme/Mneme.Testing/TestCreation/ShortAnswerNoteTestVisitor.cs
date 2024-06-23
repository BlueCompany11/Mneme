using Mneme.Integrations.GoogleBooks.Contract;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Integrations.Pluralsight.Contract;
using Mneme.Model.Interfaces;
using Mneme.Model.TestCreation;
namespace Mneme.Testing.TestCreation
{
	public class ShortAnswerNoteTestVisitor : INoteTestVisitor, INoteTestVisitor<GoogleBooksPreelaboration>, INoteTestVisitor<PluralsightPreelaboration>, INoteTestVisitor<MnemePreelaboration>
	{
		public INoteTest GetTestNote(GoogleBooksPreelaboration preelaboration)
		{
			return new ShortAnswerNoteData { Question = preelaboration.Content };
		}

		public INoteTest GetTestNote(PluralsightPreelaboration preelaboration)
		{
			return new ShortAnswerNoteData { Question = preelaboration.Content };
		}

		public INoteTest GetTestNote(MnemePreelaboration preelaboration)
		{
			return new ShortAnswerNoteData { Question = preelaboration.Content };
		}
	}
}
