using Mneme.Integrations.GoogleBooks.Contract;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Integrations.Pluralsight.Contract;
using Mneme.Model.Preelaborations;
using Mneme.PrismModule.Integrations.GoogleBooks.Views;
using Mneme.PrismModule.Integrations.Mneme.Views;
using Mneme.PrismModule.Integrations.Pluralsight.Views;
using Mneme.Views.Base;
using Prism.Regions;

namespace Mneme.PrismModule.Integration.Facade
{
	public class NoteToPreviewNavigator
	{
		private readonly IRegionManager regionManager;
		public NoteToPreviewNavigator(IRegionManager regionManager)
		{
			this.regionManager = regionManager;
		}
		public void NavigateToPreview(Preelaboration note, NavigationParameters para, string regionName)
		{
			if (note is GoogleBooksPreelaboration)
			{
				regionManager.RequestNavigate(regionName, nameof(GoogleBooksNotePreviewView), para);
			}
			else if (note is PluralsightPreelaboration)
			{
				regionManager.RequestNavigate(regionName, nameof(PluralsightNotePreviewView), para);
			}
			else if (note is MnemePreelaboration)
			{
				regionManager.RequestNavigate(regionName, nameof(MnemeNotePreviewView), para);
			}
		}
	}
}
