using Mneme.Integrations.GoogleBooks.Contract;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Integrations.Pluralsight.Contract;
using Mneme.Model.Interfaces;
using Mneme.Model.TestCreation;

namespace Mneme.Testing.TestCreation
{
	public class MultipleChoiceNoteTestVisitor : INoteTestVisitor, INoteTestVisitor<GoogleBooksNote>, INoteTestVisitor<PluralsightNote>, INoteTestVisitor<MnemeNote>
	{
		public INoteTest GetTestNote(GoogleBooksNote note)
		{
			return new MultipleChoiceNoteData { Question = note.Content };
		}

		public INoteTest GetTestNote(PluralsightNote note)
		{
			return new MultipleChoiceNoteData { Question = note.Content };
		}

		public INoteTest GetTestNote(MnemeNote note)
		{
			return new MultipleChoiceNoteData { Question = note.Content };
		}
	}
}
