using Mneme.PrismModule.Configuration.Integration.ViewModels;
using Mneme.PrismModule.Configuration.Integration.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;

namespace Mneme.PrismModule.Configuration.Integration;

public class IntegrationModule : IModule
{
	public void OnInitialized(IContainerProvider containerProvider)
	{

	}

	public void RegisterTypes(IContainerRegistry containerRegistry)
	{
		containerRegistry.RegisterForNavigation<BundledSourceConfigurationsView>();

		ViewModelLocationProvider.Register<BundledSourceConfigurationsView, BundledSourceConfigurationsViewModel>();
	}
}