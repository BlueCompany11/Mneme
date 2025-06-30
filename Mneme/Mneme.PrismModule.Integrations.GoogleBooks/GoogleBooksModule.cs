using Mneme.Core;
using Mneme.Integrations.Contracts;
using Mneme.Integrations.GoogleBooks.Authorization;
using Mneme.Integrations.GoogleBooks.Contract;
using Mneme.PrismModule.Integrations.GoogleBooks.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace Mneme.PrismModule.Integrations.GoogleBooks;

public class GoogleBooksModule : IModule
{
	public void OnInitialized(IContainerProvider containerProvider)
	{

	}

	public void RegisterTypes(IContainerRegistry containerRegistry)
	{
		containerRegistry.RegisterForNavigation<GoogleBooksNotePreviewView>();

		_ = containerRegistry.Register<GoogleBooksService>();
		_ = containerRegistry.Register<GoogleBooksNoteProvider>();
		_ = containerRegistry.Register<BaseSourcesProvider<GoogleBooksSource>, GoogleBooksSourceProvider>();

		_ = containerRegistry.Register<IIntegrationFacade<GoogleBooksSource, GoogleBooksNote>, GoogleBooksIntegrationFacade>();
		_ = containerRegistry.Register<IDatabase, GoogleBooksIntegrationFacade>();
	}
}
