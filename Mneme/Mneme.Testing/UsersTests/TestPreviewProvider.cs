using Mneme.Model.TestCreation;
using Mneme.Testing.Database;
using Mneme.Testing.RepetitionAlgorithm;

namespace Mneme.Testing.UsersTests
{
	public class TestPreviewProvider
	{
		private readonly TestTypeProvider testTypeProvider;
		private readonly SpaceRepetition spaceRepetition;

		private Queue<TestDataPreview> Tests { get; set; }
		public TestPreviewProvider(TestTypeProvider testTypeProvider, SpaceRepetition spaceRepetition)
		{
			this.testTypeProvider = testTypeProvider;
			this.spaceRepetition = spaceRepetition;
		}
		public Queue<TestDataPreview> GetTests()
		{
			return GetAllTests();
		}

		private Queue<TestDataPreview> GetAllTests()
		{
			using var testingContext = new TestingContext();
			var ret = new Queue<TestDataPreview>();
			var shortAnswers = testingContext.TestShortAnswers.ToList();
			var multipleChoice = testingContext.TestMultipleChoices.ToList();
			foreach (var item in shortAnswers)
			{
				ret.Enqueue(new TestDataPreview { Title = item.Question, CreationTime = item.Created, Type = testTypeProvider.ShortAnswer });
			}
			foreach (var item in multipleChoice)
			{
				ret.Enqueue(new TestDataPreview { Title = item.Question, CreationTime = item.Created, Type = testTypeProvider.MultipleChoice });
			}
			Tests = ret;
			return ret;
		}
		public Queue<IUserTest> GetTestsForToday()
		{
			using var testingContext = new TestingContext();
			var ret = new Queue<IUserTest>();
			var shortAnswers = testingContext.TestShortAnswers.ToList();
			var multipleChoice = testingContext.TestMultipleChoices.ToList();
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
			return ret;
		}
	}
}
