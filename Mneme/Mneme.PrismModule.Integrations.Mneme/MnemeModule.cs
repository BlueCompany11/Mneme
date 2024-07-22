using Mneme.Core;
using Mneme.Integrations.Contracts;
using Mneme.Integrations.Mneme.Contract;
using Mneme.PrismModule.Integrations.Mneme.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace Mneme.PrismModule.Integrations.Mneme;

public class MnemeModule : IModule
{
	public void OnInitialized(IContainerProvider containerProvider)
	{

	}

	public void RegisterTypes(IContainerRegistry containerRegistry)
	{
		containerRegistry.RegisterForNavigation<MnemeNotePreviewView>();

		_ = containerRegistry.Register<BaseSourcesProvider<MnemeSource>, MnemeSourcesProvider>();

		_ = containerRegistry.Register<IIntegrationFacade<MnemeSource, MnemeNote>, MnemeIntegrationFacade>();
		_ = containerRegistry.Register<IDatabase, MnemeIntegrationFacade>();
	}
}