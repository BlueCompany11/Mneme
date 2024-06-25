using Mneme.Integrations.Pluralsight;
using System.ComponentModel;
using Mneme.PrismModule.Integrations.Pluralsight.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Mneme.Integrations.Pluralsight.Contract;
using Mneme.Integrations.Contracts;

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

			containerRegistry.RegisterInstance(new PluralsightConfigProvider());
			containerRegistry.Register<PluralsightPreelaborationProviderDecorator>();

			containerRegistry.Register<PluralsightPreelaborationProvider>();
		}
	}
}