namespace Mneme.Model.TestCreation
{
	public class TestShortAnswer : Test
	{
		public string? Hint { get; set; }
		public string Answer { get; set; }

		public override string GetAnswer() => Answer;

		public override string? GetHint() => Hint;
	}
}
