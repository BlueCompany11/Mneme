using Mneme.Integrations.Pluralsight.Contract;

namespace Mneme.PrismModule.Integrations.Pluralsight
{
	public class PluralsightNotePreview
	{
		public required string Title { get; init; }
		public required string Module { get; init; }
		public required string Clip { get; init; }
		public required string Link { get; init; }
		public required string Type { get; init; }
		public required string Note { get; init; }
		public required string TimeInClip { get; init; }
		public static PluralsightNotePreview CreateFromNote(PluralsightNote note)
		{
			return new PluralsightNotePreview()
			{
				Clip = note.Clip,
				Link = note.Path,
				Module = note.Module,
				Note = note.Content,
				Title = note.Title,
				Type = "Note",
				TimeInClip = note.TimeInClip
			};
		}
	}
}
