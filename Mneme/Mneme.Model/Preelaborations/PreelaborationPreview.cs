using System;
using System.Collections.Generic;

namespace Mneme.Model.Preelaborations
{
	public class PreelaborationPreview
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string Note { get; set; }
		public DateTime Date { get; set; }
		public List<string> Tags { get; set; }
		public Preelaboration Preelaboration { get; init; }
		public static PreelaborationPreview CreateFromNote(Preelaboration note)
		{
			return new PreelaborationPreview()
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
