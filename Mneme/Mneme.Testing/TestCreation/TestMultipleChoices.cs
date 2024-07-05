namespace Mneme.Model.TestCreation
{
	public class TestMultipleChoices : IUserTest
	{
		public int NoteId { get; set; }
		public int Id { get; set; }
		public string Question { get; set; }
		public List<TestMultipleChoice> Answers { get; set; } = new();
		public int Importance { get; set; }
		public DateTime Created { get; set; }
		public TestInfo TestInfo { get; set; } = new TestInfo();
	}
}
