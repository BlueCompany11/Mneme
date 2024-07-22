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
		IReadOnlyList<TestCreation.TestShortAnswer> shortAnswers = repository.GetShortAnswerTests();
		IReadOnlyList<TestCreation.TestMultipleChoices> multipleChoice = repository.GetMultipleChoicesTests();
		foreach (TestCreation.TestShortAnswer item in shortAnswers)
		{
			ret.Add(new TestDataPreview { Title = item.Question, CreationTime = item.Created, Type = testTypeProvider.ShortAnswer });
		}
		foreach (TestCreation.TestMultipleChoices item in multipleChoice)
		{
			ret.Add(new TestDataPreview { Title = item.Question, CreationTime = item.Created, Type = testTypeProvider.MultipleChoice });
		}
		return ret;
	}
	public Queue<Test> GetTestsForToday()
	{
		var ret = new Queue<Test>();
		IReadOnlyList<TestCreation.TestShortAnswer> shortAnswers = repository.GetShortAnswerTests();
		IReadOnlyList<TestCreation.TestMultipleChoices> multipleChoice = repository.GetMultipleChoicesTests();
		foreach (TestCreation.TestShortAnswer item in shortAnswers)
		{
			if (spaceRepetition.ShouldBeTested(item))
				ret.Enqueue(item);
		}
		foreach (TestCreation.TestMultipleChoices item in multipleChoice)
		{
			if (spaceRepetition.ShouldBeTested(item))
				ret.Enqueue(item);
		}
		return ret;
	}

	Task<IReadOnlyList<Test>> ITestProvider.GetAllTests()
	{
		var ret = new List<Test>();
		IReadOnlyList<TestCreation.TestShortAnswer> shortAnswers = repository.GetShortAnswerTests();
		IReadOnlyList<TestCreation.TestMultipleChoices> multipleChoice = repository.GetMultipleChoicesTests();
		ret.AddRange(shortAnswers);
		ret.AddRange(multipleChoice);
		return Task.FromResult<IReadOnlyList<Test>>(ret);
	}

	Task<IReadOnlyList<Test>> ITestProvider.GetTestsForToday()
	{
		var ret = new List<Test>();
		IReadOnlyList<TestCreation.TestShortAnswer> shortAnswers = repository.GetShortAnswerTests();
		IReadOnlyList<TestCreation.TestMultipleChoices> multipleChoice = repository.GetMultipleChoicesTests();
		foreach (TestCreation.TestShortAnswer item in shortAnswers)
		{
			if (spaceRepetition.ShouldBeTested(item))
				ret.Add(item);
		}
		foreach (TestCreation.TestMultipleChoices item in multipleChoice)
		{
			if (spaceRepetition.ShouldBeTested(item))
				ret.Add(item);
		}
		return Task.FromResult<IReadOnlyList<Test>>(ret);
	}
}
