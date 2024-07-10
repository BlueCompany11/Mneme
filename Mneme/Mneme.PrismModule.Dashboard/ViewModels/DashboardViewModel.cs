using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mneme.Dashboard;
using Mneme.PrismModule.Integration.Facade;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Mneme.PrismModule.Dashboard.ViewModels
{
	internal class DashboardViewModel : BindableBase, INavigationAware
	{
		private CancellationTokenSource cts;
		private readonly StatisticsProvider statistics;
		private readonly DatabaseMigrations migrations;

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

		public DashboardViewModel(StatisticsProvider statistics, DatabaseMigrations migrations)
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
			catch (Microsoft.Data.Sqlite.SqliteException)
			{
				await migrations.MigrateDatabase();
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
					})
			};

			await Task.WhenAll(tasks);
		}

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return true;
		}

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{
			cts?.Cancel();
		}

		public async void OnNavigatedTo(NavigationContext navigationContext)
		{
			using (cts = new CancellationTokenSource())
			{
				var loadData = LoadData(cts.Token);
				var completedTask = await Task.WhenAny(loadData, Task.Delay(Timeout.Infinite, cts.Token));
				if(completedTask == loadData)
				{
					await loadData;
				}
			}
			cts = null;
		}
	}
}
