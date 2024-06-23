using Mneme.Dashboard;
using Mneme.PrismModule.Dashboard.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Mneme.PrismModule.Dashboard
{
	public class DashboardModule : IModule
	{
		public void OnInitialized(IContainerProvider containerProvider)
		{

		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<DashboardView>();
			containerRegistry.Register<StatisticsProvider>();
		}
	}
}