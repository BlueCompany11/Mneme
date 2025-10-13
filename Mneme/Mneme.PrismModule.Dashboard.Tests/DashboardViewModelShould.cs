using Mneme.Dashboard;
using Mneme.PrismModule.Dashboard.ViewModels;

namespace Mneme.PrismModule.Dashboard.Tests;

public class DashboardViewModelShould
{
	private readonly MockedStatisticsProvider statistics;
	private readonly DashboardViewModel sut;

	public DashboardViewModelShould()
	{
		statistics = new MockedStatisticsProvider();
		sut = new DashboardViewModel(statistics);
	}

	[Fact]
	public async Task LoadDataCommand_Executed_LoadsData()
	{
		sut.LoadDataCommand.Execute();
		await Task.Delay(10);

		MakeAssertion(statistics, sut);
	}

	[Fact]
	public async Task WhenNavigatedTo_RefreshesData()
	{
		sut.OnNavigatedTo(null);
		await Task.Delay(10);

		MakeAssertion(statistics, sut);

		statistics.KnownSourcesCount = 100;
		sut.OnNavigatedTo(null);
		await Task.Delay(10);

		MakeAssertion(statistics, sut);
	}

	private static void MakeAssertion(MockedStatisticsProvider statistics, DashboardViewModel viewModel)
	{
		Assert.Equal(statistics.KnownSourcesCount, viewModel.ActiveSourcesAmount);
		Assert.Equal(statistics.KnownNotesCount, viewModel.ActiveNotesAmount);
		Assert.Equal(statistics.MostRecentSource, viewModel.MostRecentSource);
		Assert.Equal(statistics.MostRecentNote, viewModel.MostRecentNote);
		Assert.Equal(statistics.AllTestsCount, viewModel.AllTestsCount);
		Assert.Equal(statistics.AllTestsForTestingCount, viewModel.AllTestsForTestingCount);
	}
}

internal class MockedStatisticsProvider : IStatisticsProvider
{
	public int AllTestsCount { get; set; } = 1;
	public int AllTestsForTestingCount { get; set; } = 2;
	public int KnownNotesCount { get; set; } = 3;
	public int KnownSourcesCount { get; set; } = 4;
	public string? MostRecentNote { get; set; } = "Most recent note";
	public string? MostRecentSource { get; set; } = "Most recent source";
	public Task<int> GetAllTestsCount(CancellationToken ct = default) => Task.FromResult(AllTestsCount);
	public Task<int> GetAllTestsForTestingCount(CancellationToken ct = default) => Task.FromResult(AllTestsForTestingCount);
	public Task<int> GetKnownNotesCount(CancellationToken ct = default) => Task.FromResult(KnownNotesCount);
	public Task<int> GetKnownSourcesCount(CancellationToken ct = default) => Task.FromResult(KnownSourcesCount);
	public Task<string?> GetMostRecentNote(CancellationToken ct = default) => Task.FromResult(MostRecentNote);
	public Task<string?> GetMostRecentSource(CancellationToken ct = default) => Task.FromResult(MostRecentSource);
}
