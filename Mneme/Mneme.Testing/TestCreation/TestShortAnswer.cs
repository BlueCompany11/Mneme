namespace Mneme.Model.TestCreation
{
	public class TestShortAnswer : IUserTest
	{
		public int Id { get; set; }
		public int NoteId { get; set; }
		public required string Question { get; set; }
		public required string Answer { get; set; }
		public string? Hint { get; set; }
		public int Importance { get; set; }
		public DateTime Created { get; set; }
		public TestInfo TestInfo { get; set; } = new TestInfo();
	}
}
