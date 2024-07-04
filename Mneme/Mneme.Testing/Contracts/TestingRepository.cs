using Mneme.Model.TestCreation;
using Mneme.Testing.Database;
using Mneme.Testing.UsersTests;

namespace Mneme.Testing.Contracts
{
	public class TestingRepository
	{
		private readonly TestPreviewProvider provider;

		public TestingRepository(TestPreviewProvider provider)
		{
			this.provider = provider;
		}

		public void CreateTest(TestClozeDeletion test)
		{
			using var context = new TestingContext();
			context.Add(test);
			context.SaveChanges();
		}

		public void CreateTest(TestMultipleChoices test)
		{
			using var context = new TestingContext();
			context.Add(test);
			context.SaveChanges();
		}

		public void CreateTest(TestShortAnswer test)
		{
			using var context = new TestingContext();
			context.Add(test);
			context.SaveChanges();
		}

		public void EditTest(TestClozeDeletion test)
		{
			using var context = new TestingContext();
			context.Update(test);
			context.SaveChanges();
		}

		public void EditTest(TestMultipleChoices test)
		{
			using var context = new TestingContext();
			var answersToRemove = context.TestMultipleChoice.Where(a => a.TestMultipleChoicesId == test.Id).ToList();
			context.TestMultipleChoice.RemoveRange(answersToRemove);
			context.AddRange(test.Answers);
			context.Update(test);
			context.SaveChanges();
		}

		public void EditTest(TestShortAnswer test)
		{
			using var context = new TestingContext();
			context.Update(test);
			context.SaveChanges();
		}

		public IReadOnlyList<TestDataPreview> GetTestPreviews()
		{
			return provider.GetTests().ToList();
		}

		public TestClozeDeletion GetClozeDeletionTest(string title)
		{
			using var context = new TestingContext();
			return context.TestClozeDeletions.First(t => t.Text == title);
		}

		public TestMultipleChoices GetMultipleChoicesTest(string title)
		{
			using var context = new TestingContext();
			var test = context.TestMultipleChoices.First(t => t.Question == title);
			test.Answers = context.TestMultipleChoice.Where(a => a.TestMultipleChoicesId == test.Id).ToList();
			return test;
		}

		public TestShortAnswer GetShortAnswerTest(string title)
		{
			using var context = new TestingContext();
			return context.TestShortAnswers.First(t => t.Question == title);
		}
	}
}
