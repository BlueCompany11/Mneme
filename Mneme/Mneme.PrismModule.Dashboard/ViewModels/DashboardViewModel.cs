using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mneme.Dashboard;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Mneme.PrismModule.Dashboard.ViewModels
{
	internal class DashboardViewModel : BindableBase, INavigationAware
	{
		private CancellationTokenSource cts;
		private readonly StatisticsProvider statistics;
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

		public DashboardViewModel(StatisticsProvider statistics)
		{
			this.statistics = statistics;
			LoadDataCommand = new DelegateCommand(LoadDataHandler);
		}

		private async void LoadDataHandler()
		{
			ActiveSourcesAmount = await statistics.GetKnownSourcesCount();
			ActiveNotesAmount = await statistics.GetKnownNotesCount();
			MostRecentSource = await statistics.GetMostRecentSource();
			MostRecentNote = await statistics.GetMostRecentNote();
		}

		private async Task LoadData(CancellationToken ct)
		{
			ActiveSourcesAmount = await statistics.GetKnownSourcesCount(ct);
			if (ct.IsCancellationRequested)
				return;
			ActiveNotesAmount = await statistics.GetKnownNotesCount(ct);
			if (ct.IsCancellationRequested)
				return;
			MostRecentSource = await statistics.GetMostRecentSource(ct);
			if (ct.IsCancellationRequested)
				return;
			MostRecentNote = await statistics.GetMostRecentNote(ct);
			if (ct.IsCancellationRequested)
				return;
			return;
		}

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return true;
		}

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{
			try
			{
				cts?.Cancel();
			}
			catch (System.ObjectDisposedException) { }
		}

		public async void OnNavigatedTo(NavigationContext navigationContext)
		{
			using (cts = new CancellationTokenSource())
			{
				try
				{
					await LoadData(cts.Token);
				}
				catch(TaskCanceledException) { }
			}
		}
	}
}
