using Mneme.Model;

namespace Mneme.Testing.TestCreation;

public class TestShortAnswer : ITest
{
	public string? Hint { get; set; }
	public required string Answer { get; set; }

	public override string GetAnswer() => Answer;

	public override string? GetHint() => Hint;
}
