using Mneme.Core;
using Mneme.Dashboard;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mneme.PrismModule.Dashboard.ViewModels;

internal class DashboardViewModel : BindableBase, INavigationAware
{
	private CancellationTokenSource cts;
	private readonly StatisticsProvider statistics;
	private readonly IDatabaseMigrations migrations;

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

	public DashboardViewModel(StatisticsProvider statistics, IDatabaseMigrations migrations)
	{
		this.statistics = statistics;
		this.migrations = migrations;
		LoadDataCommand = new DelegateCommand(LoadDataHandler);
	}

	private async void LoadDataHandler()
	{
		try
		{
			await LoadData(default);
		}
		catch (Exception)
		{
			await migrations.MigrateDatabases();
			await LoadData(default);
		}
	}

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
		if (MostRecentSource == default || MostRecentNote == default)
			return;
		cts?.Cancel();
	}

	public async void OnNavigatedTo(NavigationContext navigationContext)
	{
		using (cts = new CancellationTokenSource())
		{
			Task loadData = LoadData(cts.Token);
			Task completedTask = await Task.WhenAny(loadData, Task.Delay(Timeout.Infinite, cts.Token));
			if (completedTask == loadData)
			{
				await loadData;
			}
		}
		cts = null;
	}
}
