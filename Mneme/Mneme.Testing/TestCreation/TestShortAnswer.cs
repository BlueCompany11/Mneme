﻿using Mneme.Model;

namespace Mneme.Testing.TestCreation;

public class TestShortAnswer : Test
{
	public string? Hint { get; set; }
	public required string Answer { get; set; }

	public override string GetAnswer() => Answer;

	public override string? GetHint() => Hint;
}
