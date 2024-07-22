using Mneme.Dashboard;
using Mneme.PrismModule.Dashboard.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace Mneme.PrismModule.Dashboard;

public class DashboardModule : IModule
{
	public void OnInitialized(IContainerProvider containerProvider)
	{

	}

	public void RegisterTypes(IContainerRegistry containerRegistry)
	{
		containerRegistry.RegisterForNavigation<DashboardView>();
		_ = containerRegistry.Register<StatisticsProvider>();
	}
}