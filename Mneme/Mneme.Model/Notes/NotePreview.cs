using System;
using System.Collections.Generic;

namespace Mneme.Model.Notes
{
	public class NotePreview
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string Note { get; set; }
		public DateTime Date { get; set; }
		public Note BaseNote { get; init; }
		public static NotePreview CreateFromNote(Note note)
		{
			return new NotePreview()
			{
				Id = note.IntegrationId,
				Date = note.CreationTime,
				Note = note.Content,
				Title = note.Title,
				BaseNote = note
			};
		}
	}
}
