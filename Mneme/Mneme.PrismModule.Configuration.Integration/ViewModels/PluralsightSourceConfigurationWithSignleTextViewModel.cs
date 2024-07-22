using Prism.Events;

namespace Mneme.PrismModule.Configuration.Integration.ViewModels;

public class PluralsightSourceConfigurationWithSignleTextViewModel : SourceConfigurationWithSignleTextViewModel
{

	public PluralsightSourceConfigurationWithSignleTextViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
	{
		SourceName = "Pluralsight [Deprecated]";
		Text1 = "Notes in .csv format downloaded from";
		Text2 = "your Pluralsight account.";
	}
}
