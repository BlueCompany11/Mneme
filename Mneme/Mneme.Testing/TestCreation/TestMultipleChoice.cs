namespace Mneme.Testing.TestCreation
{
	public class TestMultipleChoice
	{
		public int Id { get; set; }
		public string Answer { get; set; }
		public bool IsCorrect { get; set; }
		public int TestId { get; set; }
		public TestMultipleChoices Test { get; set; }
	}
}
