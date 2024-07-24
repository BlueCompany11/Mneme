using Mneme.PrismModule.Sources.ViewModels;
using Mneme.PrismModule.Sources.Views;
using Mneme.Sources;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;

namespace Mneme.PrismModule.Sources;

public class SourcesModule : IModule
{
	public void OnInitialized(IContainerProvider containerProvider)
	{

	}

	public void RegisterTypes(IContainerRegistry containerRegistry)
	{
		containerRegistry.RegisterForNavigation<SourceCreationView>();
		containerRegistry.RegisterForNavigation<SourcesView>();
		containerRegistry.RegisterDialog<SourceCreationView, SourceCreationViewModel>();

		ViewModelLocationProvider.Register<SourceCreationView, SourceCreationViewModel>();
		ViewModelLocationProvider.Register<SourcesView, SourcesViewModel>();

		_ = containerRegistry.Register<MnemeSourceProxy>();
		_ = containerRegistry.Register<SourcesManager>();
	}
}