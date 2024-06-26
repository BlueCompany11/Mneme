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
			MnemeSourceConfigurationWithSignleTextViewModel printMedia,
			GoogleBooksSourceConfigurationViewModel googleBooks
			)
		{
			this.eventAggregator = eventAggregator;
			this.regionManager = regionManager;
			Pluralsight = pluralsight;
			MnemeSource = printMedia;
			GoogleBooks = googleBooks;
			_ = this.eventAggregator.GetEvent<NavigationRequestEvent>().Subscribe(OnRecived);
		}
		public DelegateCommand ClickCommand;

		public SourceConfigurationWithSignleTextViewModel Pluralsight { get; }
		public SourceConfigurationWithSignleTextViewModel MnemeSource { get; }
		public GoogleBooksSourceConfigurationViewModel GoogleBooks { get; }

		private void OnRecived(string msg)
		{
			string uri = "";
			if (msg == Pluralsight.SourceName)
				uri = "PluralsightConfigurationView"; //TODO
			else if (msg == MnemeSource.SourceName)
			{
				uri = "SourceCreationView"; //TODO
			}
			else if (msg == GoogleBooks.SourceName)
			{
				uri = "GoogleBooksConfigurationView"; //TODO
			}
			regionManager.RequestNavigate(RegionNames.ContentRegion, uri);
		}
	}
}
