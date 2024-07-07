using Microsoft.EntityFrameworkCore;
using Mneme.Model.TestCreation;
using Mneme.Testing.Database;
using Mneme.Testing.UsersTests;

namespace Mneme.Testing.Contracts
{
	public class TestingRepository : IDisposable
	{
		private readonly TestPreviewProvider provider;
		private readonly TestingContext context;

		public TestingRepository(TestPreviewProvider provider)
		{
			this.provider = provider;
			this.context = new();
		}

		public void CreateTest(TestMultipleChoices test)
		{
			test.Answers.ForEach(x => x.Test = test);
			context.Add(test);
			context.AddRange(test.Answers);
			context.SaveChanges();
		}

		public void CreateTest(TestShortAnswer test)
		{
			context.Add(test);
			context.SaveChanges();
		}

		public void EditTest(TestMultipleChoices test)
		{
			var answersToRemove = context.TestMultipleChoice.Where(a => a.TestId == test.Id).ToList();
			context.TestMultipleChoice.RemoveRange(answersToRemove);
			context.AddRange(test.Answers);
			context.Update(test);
			context.SaveChanges();
		}

		public void EditTest(TestShortAnswer test)
		{
			context.Update(test);
			context.SaveChanges();
		}

		public IReadOnlyList<TestDataPreview> GetTestPreviews()
		{
			return provider.GetTests().ToList();
		}

		public TestMultipleChoices GetMultipleChoicesTest(string title)
		{
			var test = context.TestMultipleChoices.Include(t => t.Answers).First(t => t.Question == title);
			return test;
		}

		public TestShortAnswer GetShortAnswerTest(string title)
		{
			return context.TestShortAnswers.First(t => t.Question == title);
		}

		public void RemoveTest(TestMultipleChoices test)
		{
			context.TestMultipleChoice.RemoveRange(test.Answers);
			context.Remove(test);
			context.SaveChanges();
		}

		public void RemoveTest(TestShortAnswer test)
		{
			context.Remove(test);
			context.SaveChanges();
		}

		public void Dispose()
		{
			context.Dispose();
		}
	}
}
