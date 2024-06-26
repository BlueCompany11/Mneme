using Mneme.Integrations.GoogleBooks.Contract;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Integrations.Pluralsight.Contract;
using Mneme.Model.Interfaces;
using Mneme.Model.TestCreation;

namespace Mneme.Testing.TestCreation
{
	public class MultipleChoiceNoteTestVisitor : INoteTestVisitor, INoteTestVisitor<GoogleBooksNote>, INoteTestVisitor<PluralsightNote>, INoteTestVisitor<MnemeNote>
	{
		public INoteTest GetTestNote(GoogleBooksNote preelaboration)
		{
			return new MultipleChoiceNoteData { Question = preelaboration.Content };
		}

		public INoteTest GetTestNote(PluralsightNote preelaboration)
		{
			return new MultipleChoiceNoteData { Question = preelaboration.Content };
		}

		public INoteTest GetTestNote(MnemeNote preelaboration)
		{
			return new MultipleChoiceNoteData { Question = preelaboration.Content };
		}
	}
}
