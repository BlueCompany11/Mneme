using System;
using System.Windows;
using DryIoc;
using MaterialDesignThemes.Wpf;
using Mneme.Core.Bootstrapper;
using Mneme.Desktop.Views;
using Mneme.PrismModule.Configuration.Integration;
using Mneme.PrismModule.Dashboard;
using Mneme.PrismModule.Dashboard.Views;
using Mneme.PrismModule.Integrations.GoogleBooks;
using Mneme.PrismModule.Integrations.Mneme;
using Mneme.PrismModule.Integrations.Pluralsight;
using Mneme.PrismModule.Notes;
using Mneme.PrismModule.Notes.Views;
using Mneme.PrismModule.Sources;
using Mneme.PrismModule.Testing;
using Mneme.Views.Base;
using Prism.DryIoc.Extensions;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;

namespace Mneme.Desktop
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		protected override Window CreateShell()
		{
			return Container.Resolve<MainWindow>();
		}

		private readonly Bootstrapper bootstrapper = new();
		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			RegisterFromBootstrapper();
			RegisterForNavigation(containerRegistry);
			MapViewWithViewModel();
			containerRegistry.RegisterSingleton<ISnackbarMessageQueue>(() => new SnackbarMessageQueue(TimeSpan.FromSeconds(4)));
			
		}

		protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
		{
			moduleCatalog.AddModule<DashboardModule>();
			moduleCatalog.AddModule<SourcesModule>();
			moduleCatalog.AddModule<NotesModule>();
			moduleCatalog.AddModule<GoogleBooksModule>();
			moduleCatalog.AddModule<MnemeModule>();
			moduleCatalog.AddModule<PluralsightModule>();
			moduleCatalog.AddModule<TestingModule>();
			moduleCatalog.AddModule<IntegrationModule>();
		}

		private static void RegisterForNavigation(IContainerRegistry containerRegistry)
		{


		}

		private void MapViewWithViewModel()
		{

		}
		private void RegisterFromBootstrapper()
		{
			RegisterFromMainContainer();
		}

		private void RegisterFromMainContainer()
		{
			var helper = new RegistrationTransferer();
			helper.TransferRegistrations(bootstrapper.Container, Container.GetContainer());
		}
		protected override void OnInitialized()
		{
			var regionManager = Container.Resolve<IRegionManager>();
			_ = regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(DashboardView));
			_ = regionManager.RegisterViewWithRegion(RegionNames.PreelaborationRegion, typeof(EmptyNotePreviewView));
			_ = regionManager.RegisterViewWithRegion(RegionNames.SideBarMenuRegion, typeof(MainMenuSideBarView));
			base.OnInitialized();
		}
	}
}
