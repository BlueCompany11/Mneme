using Mneme.Views.Base;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace Mneme.PrismModule.Configuration.Integration.ViewModels;

public class SourceConfigurationEntryBaseViewModel : BindableBase
{
	private readonly IEventAggregator eventAggregator;
	public DelegateCommand<string> NavigateRequestCommand { get; private set; }
	public string SourceName { get; set; }
	public SourceConfigurationEntryBaseViewModel(IEventAggregator eventAggregator)
	{
		this.eventAggregator = eventAggregator;
		NavigateRequestCommand = new DelegateCommand<string>(NavigateRequest);
	}

	private void NavigateRequest(string sourceName) => eventAggregator.GetEvent<NavigationRequestEvent>().Publish(sourceName);
}
