using Mneme.Integrations.Contracts;
using Mneme.Integrations.GoogleBooks.Authorization;
using Mneme.Integrations.GoogleBooks.Contract;
using Mneme.PrismModule.Integrations.GoogleBooks.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace Mneme.PrismModule.Integrations.GoogleBooks
{
	public class GoogleBooksModule : IModule
	{
		public void OnInitialized(IContainerProvider containerProvider)
		{

		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<GoogleBooksNotePreviewView>();

			containerRegistry.Register<GoogleCredentialsProvider>();
			containerRegistry.Register<GoogleBooksService>();
			containerRegistry.Register<GoogleBooksPreelaborationProvider>();
			containerRegistry.Register<BaseSourcesProvider<GoogleBooksSource>, GoogleBooksSourceProvider>();

		}
	}
}