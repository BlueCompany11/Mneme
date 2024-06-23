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
		}
	}
}