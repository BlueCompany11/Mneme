using System;
using System.Collections.Generic;

namespace Mneme.Model.Preelaborations
{
	public class NotePreview
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string Note { get; set; }
		public DateTime Date { get; set; }
		public List<string> Tags { get; set; }
		public Note Preelaboration { get; init; }
		public static NotePreview CreateFromNote(Note note)
		{
			return new NotePreview()
			{
				Id = note.IntegrationId,
				Date = note.CreationTime,
				Note = note.Content,
				Title = note.Title,
				Tags = [],
				Preelaboration = note
			};
		}
	}
}
