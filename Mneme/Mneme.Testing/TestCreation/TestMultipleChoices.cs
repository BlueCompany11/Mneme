﻿using Mneme.Model;

namespace Mneme.Testing.TestCreation;

public class TestMultipleChoices : Test
{
	public List<TestMultipleChoice> Answers { get; set; } = [];

	public override string GetAnswer()
	{
		ShuffleAnswers();
		var answer = "";
		for (var i = 0; i < Answers.Count; i++)
		{
			if (Answers[i].IsCorrect)
				answer += $"{(char)('A' + i)}: {Answers[i].Answer},";
		}
		return answer.TrimEnd(',');

		void ShuffleAnswers()
		{
			var random = new Random();
			Answers = Answers.OrderBy(x => random.Next()).Where(x => !string.IsNullOrEmpty(x.Answer)).ToList();
		}
	}

	public override string? GetHint() => GenerateHint();

	private string GenerateHint()
	{
		var hint = "";
		for (var i = 0; i < Answers.Count; i++)
		{
			hint += $"{(char)('A' + i)}: {Answers[i].Answer} ";
		}
		return hint.Trim();
	}
}
