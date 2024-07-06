using Mneme.PrismModule.Configuration.Integration.Views;
using Mneme.Views.Base;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace Mneme.PrismModule.Configuration.Integration.ViewModels
{
	public class BundledSourceConfigurationsViewModel : BindableBase
	{
		private readonly IEventAggregator eventAggregator;
		private readonly IRegionManager regionManager;

		public BundledSourceConfigurationsViewModel(IEventAggregator eventAggregator, IRegionManager regionManager, PluralsightSourceConfigurationWithSignleTextViewModel pluralsight,
			GoogleBooksSourceConfigurationViewModel googleBooks
			)
		{
			this.eventAggregator = eventAggregator;
			this.regionManager = regionManager;
			Pluralsight = pluralsight;
			GoogleBooks = googleBooks;
			_ = this.eventAggregator.GetEvent<NavigationRequestEvent>().Subscribe(OnRecived);
		}
		public DelegateCommand ClickCommand;

		public SourceConfigurationWithSignleTextViewModel Pluralsight { get; }
		public GoogleBooksSourceConfigurationViewModel GoogleBooks { get; }

		private void OnRecived(string msg)
		{
			string uri = "";
			if (msg == Pluralsight.SourceName)
				uri = nameof(PluralsightConfigurationView);
			else if (msg == GoogleBooks.SourceName)
			{
				uri = nameof(GoogleBooksConfigurationView);
			}
			regionManager.RequestNavigate(RegionNames.ContentRegion, uri);
		}
	}
}
