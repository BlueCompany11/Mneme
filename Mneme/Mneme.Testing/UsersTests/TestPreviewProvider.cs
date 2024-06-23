using Mneme.Model.TestCreation;
using Mneme.Testing.Database;
using Mneme.Testing.RepetitionAlgorithm;

namespace Mneme.Testing.UsersTests
{
	public class TestPreviewProvider
	{
		private readonly TestingContext testingContext;
		private readonly TestTypeProvider testTypeProvider;
		private readonly SpaceRepetition spaceRepetition;

		private Queue<TestDataPreview> Tests { get; set; }
		public TestPreviewProvider(TestingContext testingContext, TestTypeProvider testTypeProvider, SpaceRepetition spaceRepetition)
		{
			this.testingContext = testingContext;
			this.testTypeProvider = testTypeProvider;
			this.spaceRepetition = spaceRepetition;
		}
		public Queue<TestDataPreview> GetTests()
		{
			return GetAllTests();
		}

		private Queue<TestDataPreview> GetAllTests()
		{
			var ret = new Queue<TestDataPreview>();
			var shortAnswers = testingContext.TestShortAnswers.ToList();
			var multipleChoice = testingContext.TestMultipleChoices.ToList();
			var clozeDeletions = testingContext.TestClozeDeletions.ToList();
			foreach (var item in shortAnswers)
			{
				ret.Enqueue(new TestDataPreview { Title = item.Question, CreationTime = item.Created, Type = testTypeProvider.ShortAnswer });
			}
			foreach (var item in multipleChoice)
			{
				ret.Enqueue(new TestDataPreview { Title = item.Question, CreationTime = item.Created, Type = testTypeProvider.MultipleChoice });
			}
			foreach (var item in clozeDeletions)
			{
				ret.Enqueue(new TestDataPreview { Title = item.Text, CreationTime = item.Created, Type = testTypeProvider.ClozeDeletion });
			}
			Tests = ret;
			return ret;
		}
		public Queue<IUserTest> GetTestsForToday()
		{
			var ret = new Queue<IUserTest>();
			var shortAnswers = testingContext.TestShortAnswers.ToList();
			var multipleChoice = testingContext.TestMultipleChoices.ToList();
			var clozeDeletions = testingContext.TestClozeDeletions.ToList();
			foreach (var item in shortAnswers)
			{
				if (spaceRepetition.ShouldBeTested(item))
					ret.Enqueue(item);
			}
			foreach (var item in multipleChoice)
			{
				if (spaceRepetition.ShouldBeTested(item))
					ret.Enqueue(item);
			}
			foreach (var item in clozeDeletions)
			{
				if (spaceRepetition.ShouldBeTested(item))
					ret.Enqueue(item);
			}
			return ret;
		}
	}
}
