using Mneme.Integrations.GoogleBooks.Contract;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Integrations.Pluralsight.Contract;
using Mneme.Model;
using Mneme.PrismModule.Integrations.GoogleBooks.Views;
using Mneme.PrismModule.Integrations.Mneme.Views;
using Mneme.PrismModule.Integrations.Pluralsight.Views;
using Prism.Regions;

namespace Mneme.PrismModule.Integration.Facade;

public class NoteToPreviewNavigator
{
	private readonly IRegionManager regionManager;
	public NoteToPreviewNavigator(IRegionManager regionManager) => this.regionManager = regionManager;
	public void NavigateToPreview(Note note, NavigationParameters para, string regionName)
	{
		if (note is GoogleBooksNote)
		{
			regionManager.RequestNavigate(regionName, nameof(GoogleBooksNotePreviewView), para);
		} else if (note is PluralsightNote)
		{
			regionManager.RequestNavigate(regionName, nameof(PluralsightNotePreviewView), para);
		} else if (note is MnemeNote)
		{
			regionManager.RequestNavigate(regionName, nameof(MnemeNotePreviewView), para);
		}
	}
}
