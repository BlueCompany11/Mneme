namespace Mneme.Testing.TestCreation;

public class TestMultipleChoice
{
	public int Id { get; set; }
	public required string Answer { get; set; }
	public bool IsCorrect { get; set; }
	public int TestId { get; set; }
	public required TestMultipleChoices Test { get; set; }
}
