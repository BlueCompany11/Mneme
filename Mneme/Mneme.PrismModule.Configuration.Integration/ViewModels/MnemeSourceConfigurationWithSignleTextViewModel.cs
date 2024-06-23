using Prism.Events;

namespace Mneme.PrismModule.Configuration.Integration.ViewModels
{
	public class MnemeSourceConfigurationWithSignleTextViewModel : SourceConfigurationWithSignleTextViewModel
	{
		public MnemeSourceConfigurationWithSignleTextViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
		{
			SourceName = "Mneme sources";
			Text1 = "Sources that can't be integrated";
		}
	}
}
