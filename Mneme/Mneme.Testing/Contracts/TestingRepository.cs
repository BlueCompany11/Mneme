using Microsoft.EntityFrameworkCore;
using Mneme.Model;
using Mneme.Testing.Database;
using Mneme.Testing.TestCreation;

namespace Mneme.Testing.Contracts;

public class TestingRepository
{
	public void CreateTest(TestMultipleChoices test)
	{
		using TestingContext context = new();
		test.Answers.ForEach(x => x.Test = test);
		_ = context.Add(test);
		context.AddRange(test.Answers);
		_ = context.SaveChanges();
	}

	public void CreateTest(TestShortAnswer test)
	{
		using TestingContext context = new();
		_ = context.Add(test);
		_ = context.SaveChanges();
	}

	public void EditTest(TestMultipleChoices test)
	{
		using TestingContext context = new();
		var answersToRemove = context.TestMultipleChoice.Where(a => a.TestId == test.Id).ToList();
		context.TestMultipleChoice.RemoveRange(answersToRemove);
		context.AddRange(test.Answers);
		_ = context.Update(test);
		_ = context.SaveChanges();
	}

	public void EditTest(TestShortAnswer test)
	{
		using TestingContext context = new();
		_ = context.Update(test);
		_ = context.SaveChanges();
	}

	public void EditTest(ITest test)
	{
		using TestingContext context = new();
		_ = context.Update(test);
		_ = context.SaveChanges();
	}

	public TestMultipleChoices? GetMultipleChoicesTest(string title)
	{
		using TestingContext context = new();
		return context.TestMultipleChoices.Include(t => t.Answers).FirstOrDefault(t => t.Question == title);
	}

	public TestShortAnswer? GetShortAnswerTest(string title)
	{
		using TestingContext context = new();
		return context.TestShortAnswers.FirstOrDefault(t => t.Question == title);
	}

	public IReadOnlyList<TestMultipleChoices> GetMultipleChoicesTests()
	{
		using TestingContext context = new();
		return context.TestMultipleChoices.Include(t => t.Answers).ToList();
	}

	public IReadOnlyList<TestShortAnswer> GetShortAnswerTests()
	{
		using TestingContext context = new();
		return context.TestShortAnswers.ToList();
	}

	public void RemoveTest(TestMultipleChoices test)
	{
		using TestingContext context = new();
		context.TestMultipleChoice.RemoveRange(test.Answers);
		_ = context.Remove(test);
		_ = context.SaveChanges();
	}

	public void RemoveTest(TestShortAnswer test)
	{
		using TestingContext context = new();
		_ = context.Remove(test);
		_ = context.SaveChanges();
	}
}
