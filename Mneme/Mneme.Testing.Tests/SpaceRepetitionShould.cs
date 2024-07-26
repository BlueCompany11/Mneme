using AutoFixture;
using Mneme.Testing.RepetitionAlgorithm;
using Mneme.Testing.TestCreation;
using Mneme.Tests.Base;

namespace Mneme.Testing.Tests;

public class SpaceRepetitionShould : BaseTest
{
	[Fact]
	public void ScheduleTestForNewlyCreatedTests()
	{
		fixture.Customize<TestShortAnswer>(c => c
			.With(t => t.Created, DateTime.Now)
			.With(t => t.Interval, 0));
		var test = fixture.Create<TestShortAnswer>();

		var sut = fixture.Create<SpaceRepetition>();

		Assert.True(sut.ShouldBeTested(test));
	}

	[Fact]
	public void NotScheduleTest_WhenThereAreManyDaysInSchedule()
	{
		fixture.Customize<TestShortAnswer>(c => c
			.With(t => t.Created, DateTime.Now)
			.With(t => t.Interval, 100));
		var test = fixture.Create<TestShortAnswer>();

		var sut = fixture.Create<SpaceRepetition>();

		Assert.True(sut.ShouldBeTested(test));
	}
}
