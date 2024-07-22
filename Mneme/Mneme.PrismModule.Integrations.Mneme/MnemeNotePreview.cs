using Mneme.Integrations.Mneme.Contract;
using System;

namespace Mneme.PrismModule.Integrations.Mneme;

public class MnemeNotePreview
{
	public required string Title { get; init; }
	public required string NoteText { get; init; }
	public required string NoteDetails { get; init; }
	public required string Source { get; init; }
	public required string SourceDetails { get; init; }
	public required DateTime CreationDate { get; init; }
	public static MnemeNotePreview CreateFromNote(MnemeNote note)
	{
		return new MnemeNotePreview()
		{
			Title = note.Title,
			NoteText = note.Content,
			NoteDetails = note.Path,
			Source = note.Source.Title,
			SourceDetails = note.Source.Details,
			CreationDate = note.CreationTime
		};
	}
}
