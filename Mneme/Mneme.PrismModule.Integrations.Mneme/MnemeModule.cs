using Mneme.PrismModule.Integrations.Mneme.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Mneme.PrismModule.Integrations.Mneme
{
	public class MnemeModule : IModule
	{
		public void OnInitialized(IContainerProvider containerProvider)
		{

		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<MnemeNotePreviewView>();
		}
	}
}