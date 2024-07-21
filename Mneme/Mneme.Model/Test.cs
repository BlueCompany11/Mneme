using System;

namespace Mneme.Model
{
	public abstract class Test
	{
		public int Id { get; init; }
		public int? NoteId { get; set; }
		public string Question { get; set; }
		public DateTime Created { get; init; }
		public DateTime Updated { get; set; }
		public int Interval { get; set; }
		public int Importance { get; set; }
		public abstract string GetAnswer();
		public abstract string GetHint();
	}
}
