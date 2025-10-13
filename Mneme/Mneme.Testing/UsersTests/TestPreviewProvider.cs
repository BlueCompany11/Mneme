using Mneme.Core;
using Mneme.Model;
using Mneme.Testing.Contracts;
using Mneme.Testing.RepetitionAlgorithm;

namespace Mneme.Testing.UsersTests;

public class TestPreviewProvider : ITestProvider
{
	private readonly TestTypeProvider testTypeProvider;
	private readonly TestingRepository repository;
	private readonly SpaceRepetition spaceRepetition;

	public TestPreviewProvider(TestTypeProvider testTypeProvider, TestingRepository repository, SpaceRepetition spaceRepetition)
	{
		this.testTypeProvider = testTypeProvider;
		this.repository = repository;
		this.spaceRepetition = spaceRepetition;
	}

	public IReadOnlyList<TestDataPreview> GetAllTests()
	{
		var ret = new List<TestDataPreview>();
		var shortAnswers = repository.GetShortAnswerTests();
		var multipleChoice = repository.GetMultipleChoicesTests();
		foreach (var item in shortAnswers)
		{
			ret.Add(new TestDataPreview { Title = item.Question, CreationTime = item.Created, Type = testTypeProvider.ShortAnswer });
		}
		foreach (var item in multipleChoice)
		{
			ret.Add(new TestDataPreview { Title = item.Question, CreationTime = item.Created, Type = testTypeProvider.MultipleChoice });
		}
		return ret;
	}
	public Queue<Test> GetTestsForToday()
	{
		var ret = new Queue<Test>();
		var shortAnswers = repository.GetShortAnswerTests();
		var multipleChoice = repository.GetMultipleChoicesTests();
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

	Task<IReadOnlyList<Test>> ITestProvider.GetAllTests(CancellationToken ct)
	{
		var ret = new List<Test>();
		var shortAnswers = repository.GetShortAnswerTests();
		var multipleChoice = repository.GetMultipleChoicesTests();
		ret.AddRange(shortAnswers);
		ret.AddRange(multipleChoice);
		return Task.FromResult<IReadOnlyList<Test>>(ret);
	}

	Task<IReadOnlyList<Test>> ITestProvider.GetTestsForToday(CancellationToken ct)
	{
		var ret = new List<Test>();
		var shortAnswers = repository.GetShortAnswerTests();
		var multipleChoice = repository.GetMultipleChoicesTests();
		foreach (var item in shortAnswers)
		{
			if (spaceRepetition.ShouldBeTested(item))
				ret.Add(item);
		}
		foreach (var item in multipleChoice)
		{
			if (spaceRepetition.ShouldBeTested(item))
				ret.Add(item);
		}
		return Task.FromResult<IReadOnlyList<Test>>(ret);
	}
}
