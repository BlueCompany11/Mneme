using Mneme.PrismModule.Integrations.Pluralsight.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

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
		}
	}
}