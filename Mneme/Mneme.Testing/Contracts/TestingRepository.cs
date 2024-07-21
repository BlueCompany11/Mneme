using Microsoft.EntityFrameworkCore;
using Mneme.Model;
using Mneme.Model.TestCreation;
using Mneme.Testing.Database;

namespace Mneme.Testing.Contracts
{
	public class TestingRepository
	{
		public void CreateTest(TestMultipleChoices test)
		{
			using TestingContext context = new();
			test.Answers.ForEach(x => x.Test = test);
			context.Add(test);
			context.AddRange(test.Answers);
			context.SaveChanges();
		}

		public void CreateTest(TestShortAnswer test)
		{
			using TestingContext context = new();
			context.Add(test);
			context.SaveChanges();
		}

		public void EditTest(TestMultipleChoices test)
		{
			using TestingContext context = new();
			var answersToRemove = context.TestMultipleChoice.Where(a => a.TestId == test.Id).ToList();
			context.TestMultipleChoice.RemoveRange(answersToRemove);
			context.AddRange(test.Answers);
			context.Update(test);
			context.SaveChanges();
		}

		public void EditTest(TestShortAnswer test)
		{
			using TestingContext context = new();
			context.Update(test);
			context.SaveChanges();
		}

		public void EditTest(Test test)
		{
			using TestingContext context = new();
			context.Update(test);
			context.SaveChanges();
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
			context.Remove(test);
			context.SaveChanges();
		}

		public void RemoveTest(TestShortAnswer test)
		{
			using TestingContext context = new();
			context.Remove(test);
			context.SaveChanges();
		}
	}
}
