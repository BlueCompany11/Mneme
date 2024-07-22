using Prism.Events;

namespace Mneme.PrismModule.Configuration.Integration.ViewModels;

public class SourceConfigurationWithSignleTextViewModel : SourceConfigurationEntryBaseViewModel
{
	public SourceConfigurationWithSignleTextViewModel(IEventAggregator eventAggregator) : base(eventAggregator) { }
	public string Text1 { get; set; }
	public string Text2 { get; set; }
}
