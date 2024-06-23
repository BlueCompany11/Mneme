using Prism.Events;

namespace Mneme.PrismModule.Configuration.Integration.ViewModels
{
	public class GoogleBooksSourceConfigurationViewModel : SourceConfigurationEntryBaseViewModel
	{
		public string Format1 { get; set; }
		public string Format2 { get; set; }
		public string Format3 { get; set; }
		public string Format4 { get; set; }
		public bool IsButtonEnabled { get; set; } = true;
		public string ToolTip { get; set; } = "";
		public GoogleBooksSourceConfigurationViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
		{
			SourceName = "Google Books";
			Format1 = ".epub";
			Format2 = ".pdf";
		}
	}
}
