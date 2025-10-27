using Mneme.PrismModule.Configuration.Integration.Views;
using Mneme.PrismModule.Dashboard.Views;
using Mneme.PrismModule.Notes.Views;
using Mneme.PrismModule.Sources.Views;
using Mneme.PrismModule.Testing.Views.UsersTests;
using Mneme.Views.Base;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation.Regions;

namespace Mneme.Desktop.ViewModels;

public class MainMenuSideBarViewModel : BindableBase
{
	private readonly IRegionManager regionManager;
	public string NavigateToDashboardParam => nameof(DashboardView);
	public string NavigateToSourcesParam => nameof(SourcesView);
	public string NavigateToNotesParam => nameof(NotesView);
	public string NavigateToTestingParam => nameof(TestsView);
	public string NavigateToIntegrationParam => nameof(BundledSourceConfigurationsView);
	public DelegateCommand<string> NavigateCommand { get; private set; }
	public MainMenuSideBarViewModel(IRegionManager regionManager)
	{
		this.regionManager = regionManager;
		NavigateCommand = new DelegateCommand<string>(
			(url) => this.regionManager.RequestNavigate(RegionNames.ContentRegion, url));
	}
}
