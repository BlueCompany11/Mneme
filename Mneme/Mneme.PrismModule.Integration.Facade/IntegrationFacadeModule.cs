using Mneme.Core;
using Prism.Ioc;
using Prism.Modularity;

namespace Mneme.PrismModule.Integration.Facade
{
	public class IntegrationFacadeModule : IModule
	{
		public void OnInitialized(IContainerProvider containerProvider)
		{

		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.Register<IBundledIntegrationFacades, BundledIntegrationFacades>();
			containerRegistry.RegisterSingleton<IDatabaseMigrations, DatabaseMigrations>();
		}
	}
}
