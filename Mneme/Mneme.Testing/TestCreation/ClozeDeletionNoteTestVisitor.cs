using Mneme.Integrations.GoogleBooks.Contract;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Integrations.Pluralsight.Contract;
using Mneme.Model.Interfaces;
using Mneme.Model.TestCreation;

namespace Mneme.Testing.TestCreation
{
	public class ClozeDeletionNoteTestVisitor : INoteTestVisitor, INoteTestVisitor<GoogleBooksNote>, INoteTestVisitor<PluralsightNote>, INoteTestVisitor<MnemeNote>
	{
		public INoteTest GetTestNote(GoogleBooksNote preelaboration)
		{
			return new ClozeDeletionNoteData { Text = preelaboration.Content };
		}

		public INoteTest GetTestNote(PluralsightNote preelaboration)
		{
			return new ClozeDeletionNoteData { Text = preelaboration.Content };
		}

		public INoteTest GetTestNote(MnemeNote preelaboration)
		{
			return new ClozeDeletionNoteData { Text = preelaboration.Content };
		}
	}
}
