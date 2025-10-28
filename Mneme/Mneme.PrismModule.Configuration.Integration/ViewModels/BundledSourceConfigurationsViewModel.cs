using Mneme.Views.Base;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation.Regions;

namespace Mneme.PrismModule.Configuration.Integration.ViewModels;

public class BundledSourceConfigurationsViewModel : BindableBase
{
	private readonly IEventAggregator eventAggregator;
	private readonly IRegionManager regionManager;

	public BundledSourceConfigurationsViewModel(IEventAggregator eventAggregator, IRegionManager regionManager
		)
	{
		this.eventAggregator = eventAggregator;
		this.regionManager = regionManager;
		_ = this.eventAggregator.GetEvent<NavigationRequestEvent>().Subscribe(OnRecived);
	}
	public DelegateCommand ClickCommand;

	public SourceConfigurationWithSignleTextViewModel Pluralsight { get; }

	private void OnRecived(string msg)
	{
		var uri = "";
		regionManager.RequestNavigate(RegionNames.ContentRegion, uri);
	}
}
