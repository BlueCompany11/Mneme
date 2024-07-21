using Mneme.Core;
using Mneme.Integrations.Contracts;
using Mneme.Integrations.Pluralsight;
using Mneme.Integrations.Pluralsight.Contract;
using Mneme.PrismModule.Integrations.Pluralsight.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace Mneme.PrismModule.Integrations.Pluralsight
{
	public class PluralsightModule : IModule
	{
		public void OnInitialized(IContainerProvider containerProvider)
		{

		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<PluralsightNotePreviewView>();

			containerRegistry.Register<PluralsightConfigProvider>();
			containerRegistry.Register<PluralsightNoteProviderDecorator>();
			containerRegistry.Register<BaseSourcesProvider<PluralsightSource>, PluralsightSourceProvider>();
			containerRegistry.Register<IIntegrationFacade<PluralsightSource, PluralsightNote>, PluralsightIntegrationFacade>();

			containerRegistry.Register<IDatabase, PluralsightIntegrationFacade>();
		}
	}
}