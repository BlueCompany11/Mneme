namespace Mneme.Model.TestCreation
{
	public abstract class Test
	{
		public int Id { get; init; }
		public int? NoteId { get; set; }
		public string Question { get; set; }
		public DateTime Created { get; init; }
		public DateTime Updated { get; set; } = DateTime.Now;
		public int Interval { get; set; }
		public int Importance { get; set; }
		public abstract string GetAnswer();
		public abstract string? GetHint();
	}
}
