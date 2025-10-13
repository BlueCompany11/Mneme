using Mneme.Dashboard;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mneme.PrismModule.Dashboard.ViewModels;

public class DashboardViewModel : BindableBase, INavigationAware
{
	private CancellationTokenSource cts;
	private readonly IStatisticsProvider statistics;

	public DelegateCommand LoadDataCommand { get; }

	private int activeSourcesAmount;

	public int ActiveSourcesAmount
	{
		get => activeSourcesAmount;
		set => SetProperty(ref activeSourcesAmount, value);
	}
	private int activeNotesAmount;

	public int ActiveNotesAmount
	{
		get => activeNotesAmount;
		set => SetProperty(ref activeNotesAmount, value);
	}
	private string activeSourceName;

	public string MostRecentSource
	{
		get => activeSourceName;
		set => SetProperty(ref activeSourceName, value);
	}

	private string mostRecentNote;

	public string MostRecentNote
	{
		get => mostRecentNote;
		set => SetProperty(ref mostRecentNote, value);
	}

	private int allTestsCount;

	public int AllTestsCount
	{
		get => allTestsCount;
		set => SetProperty(ref allTestsCount, value);
	}

	private int allTestsForTestingCount;

	public int AllTestsForTestingCount
	{
		get => allTestsForTestingCount;
		set => SetProperty(ref allTestsForTestingCount, value);
	}

	public DashboardViewModel(IStatisticsProvider statistics)
	{
		this.statistics = statistics;
		LoadDataCommand = new DelegateCommand(LoadDataHandler);
	}

	private async void LoadDataHandler() => await LoadData(default);

	private async Task LoadData(CancellationToken ct)
	{
		var tasks = new List<Task>
			{
				Task.Run(async () =>
				{
						ActiveSourcesAmount = await statistics.GetKnownSourcesCount(ct);
				}),
				Task.Run(async () =>
				{
						ActiveNotesAmount = await statistics.GetKnownNotesCount(ct);
				}),
				Task.Run(async () =>
				{
						MostRecentSource = await statistics.GetMostRecentSource(ct);
				}),
				Task.Run(async () =>
				{
						MostRecentNote = await statistics.GetMostRecentNote(ct);
				}),
				Task.Run(async () =>
				{
						AllTestsCount = await statistics.GetAllTestsCount(ct);
				}),
				Task.Run(async () =>
				{
						AllTestsForTestingCount = await statistics.GetAllTestsForTestingCount(ct);
				}),
			};

		await Task.WhenAll(tasks);
	}

	public bool IsNavigationTarget(NavigationContext navigationContext) => true;

	public void OnNavigatedFrom(NavigationContext navigationContext)
	{
		if (MostRecentSource == default || MostRecentNote == default) // don't cancel if data was never loaded
			return;
		cts?.Cancel();
	}

	public async void OnNavigatedTo(NavigationContext navigationContext)
	{
		using (cts = new CancellationTokenSource())
		{
			var loadData = LoadData(cts.Token);
			var completedTask = await Task.WhenAny(loadData, Task.Delay(Timeout.Infinite, cts.Token));
			if (completedTask == loadData)
			{
				await loadData;
			}
		}
		cts = null;
	}
}
