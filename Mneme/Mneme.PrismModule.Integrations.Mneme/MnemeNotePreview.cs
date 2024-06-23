using System;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Model.Interfaces;

namespace Mneme.PrismModule.Integrations.Mneme
{
	public class MnemeNotePreview
	{
		public required string Title { get; init; }
		public required string NoteText { get; init; }
		public required string NoteDetails { get; init; }
		public required string Source { get; init; }
		public required string SourceDetails { get; init; }
		public required DateTime CreationDate { get; init; }
		public static MnemeNotePreview CreateFromNote(MnemePreelaboration preelaboration)
		{
			return new MnemeNotePreview()
			{
				Title = preelaboration.Title,
				NoteText = preelaboration.Content,
				NoteDetails = preelaboration.Path,
				Source = preelaboration.Source.Title,
				SourceDetails = preelaboration.Source.Details,
				CreationDate = preelaboration.CreationTime
			};
		}
	}
}
