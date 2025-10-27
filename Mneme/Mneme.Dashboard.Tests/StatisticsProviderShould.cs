using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Mneme.Core;
using Mneme.Model;
using Moq;
using System.Collections.Immutable;

namespace Mneme.Dashboard.Tests;
public class StatisticsProviderShould
{
	private readonly ITestProvider testProvider;
	private readonly IBundledIntegrationFacades integration;
	private readonly IReadOnlyList<Note> notes;
	private readonly IReadOnlyList<ISource> sources;
	private readonly IReadOnlyList<Test> tests;
	private IFixture fixture;
	public StatisticsProviderShould()
	{
		fixture = new Fixture().Customize(new AutoMoqCustomization());
		notes = fixture.CreateMany<Note>().ToImmutableList();
		sources = fixture.CreateMany<ISource>().ToImmutableList();
		integration = Mock.Of<IBundledIntegrationFacades>(x =>
		x.GetKnownSources(true, default) == Task.FromResult(sources) &&
		x.GetKnownNotes(true, default) == Task.FromResult(notes));

		tests = fixture.CreateMany<Test>().ToImmutableList();
		testProvider = Mock.Of<ITestProvider>(x =>
		x.GetTestsForToday(default) == Task.FromResult(tests) &&
		x.GetAllTests(default) == Task.FromResult(tests));
	}

	[Fact]
	public async Task GetKnownSourcesCount_ShouldReturnCorrectCount()
	{
		var sut = new StatisticsProvider(integration, testProvider);

		var actualCount = await sut.GetKnownSourcesCount();

		_ = actualCount.Should().Be(sources.Count);
	}

	[Fact]
	public async Task GetKnownNotesCount_ShouldReturnCorrectCount()
	{
		var sut = new StatisticsProvider(integration, testProvider);

		var actualCount = await sut.GetKnownNotesCount();

		_ = actualCount.Should().Be(notes.Count);
	}

	[Fact]
	public async Task GetMostRecentSource_ShouldReturnMostRecentSource()
	{
		var expectedSource = sources.OrderBy(x => x.CreationTime).First();
		var sut = new StatisticsProvider(integration, testProvider);

		var actualSource = await sut.GetMostRecentSource();

		_ = actualSource.Should().Contain(expectedSource.Title);
	}

	[Fact]
	public async Task GetMostRecentNote_ShouldReturnMostRecentNote()
	{
		var expectedNote = notes.OrderBy(x => x.CreationTime).First();
		var sut = new StatisticsProvider(integration, testProvider);

		var actualNote = await sut.GetMostRecentNote();

		_ = actualNote.Should().Contain(expectedNote.Title);
	}

	[Fact]
	public async Task GetAllTestsCount_ShouldReturnCorrectCount()
	{
		var sut = new StatisticsProvider(integration, testProvider);

		var actualCount = await sut.GetAllTestsCount();

		_ = actualCount.Should().Be(tests.Count);
	}

	[Fact]
	public async Task GetAllTestsForTestingCount_ShouldReturnCorrectCount()
	{
		var sut = new StatisticsProvider(integration, testProvider);

		var actualCount = await sut.GetAllTestsForTestingCount();

		_ = actualCount.Should().Be(tests.Count);
	}
}
