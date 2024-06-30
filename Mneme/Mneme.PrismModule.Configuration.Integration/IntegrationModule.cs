using Mneme.PrismModule.Configuration.Integration.BusinessLogic;
using Mneme.PrismModule.Configuration.Integration.ViewModels;
using Mneme.PrismModule.Configuration.Integration.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;

namespace Mneme.PrismModule.Configuration.Integration
{
	public class IntegrationModule : IModule
	{
		public void OnInitialized(IContainerProvider containerProvider)
		{

		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<BundledSourceConfigurationsView>();
			containerRegistry.RegisterForNavigation<PluralsightConfigurationView>();
			containerRegistry.RegisterForNavigation<GoogleBooksConfigurationView>();

			ViewModelLocationProvider.Register<PluralsightConfigurationView, PluralsightConfigurationViewModel>();
			ViewModelLocationProvider.Register<GoogleBooksConfigurationView, GoogleBooksSourceConfigurationViewModel>();
			ViewModelLocationProvider.Register<BundledSourceConfigurationsView, BundledSourceConfigurationsViewModel>();

			containerRegistry.Register<GoogleBooksConnector>();
		}
	}
}