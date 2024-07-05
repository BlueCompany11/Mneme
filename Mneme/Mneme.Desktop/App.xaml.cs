using System;
using System.Windows;
using DryIoc;
using MaterialDesignThemes.Wpf;
using Mneme.Desktop.Views;
using Mneme.PrismModule.Configuration.Integration;
using Mneme.PrismModule.Dashboard;
using Mneme.PrismModule.Dashboard.Views;
using Mneme.PrismModule.Integration.Facade;
using Mneme.PrismModule.Integrations.Base;
using Mneme.PrismModule.Integrations.GoogleBooks;
using Mneme.PrismModule.Integrations.Mneme;
using Mneme.PrismModule.Integrations.Pluralsight;
using Mneme.PrismModule.Notes;
using Mneme.PrismModule.Notes.Views;
using Mneme.PrismModule.Sources;
using Mneme.PrismModule.Testing;
using Mneme.Views.Base;
using Prism.Ioc;
using Prism.Modularity;
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

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterSingleton<ISnackbarMessageQueue>(() => new SnackbarMessageQueue(TimeSpan.FromSeconds(4)));
		}

		protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
		{
			moduleCatalog.AddModule<DashboardModule>(InitializationMode.WhenAvailable);
			moduleCatalog.AddModule<SourcesModule>(InitializationMode.WhenAvailable);
			moduleCatalog.AddModule<NotesModule>(InitializationMode.WhenAvailable);
			moduleCatalog.AddModule<GoogleBooksModule>(InitializationMode.WhenAvailable);
			moduleCatalog.AddModule<MnemeModule>(InitializationMode.WhenAvailable);
			moduleCatalog.AddModule<PluralsightModule>(InitializationMode.WhenAvailable);
			moduleCatalog.AddModule<TestingModule>(InitializationMode.WhenAvailable);
			moduleCatalog.AddModule<IntegrationModule>(InitializationMode.WhenAvailable);
			moduleCatalog.AddModule<BaseModule>(InitializationMode.WhenAvailable);
			moduleCatalog.AddModule<IntegrationFacadeModule>(InitializationMode.WhenAvailable);
			moduleCatalog.AddModule<IntegrationBaseModule>(InitializationMode.WhenAvailable);
		}

		protected override void OnInitialized()
		{
			var regionManager = Container.Resolve<IRegionManager>();
			_ = regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(DashboardView));
			_ = regionManager.RegisterViewWithRegion(RegionNames.NoteRegion, typeof(EmptyNotePreviewView));
			_ = regionManager.RegisterViewWithRegion(RegionNames.SideBarMenuRegion, typeof(MainMenuSideBarView));
			base.OnInitialized();
		}

		protected override async void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			var migrations = Container.Resolve<DatabaseMigrations>();
			await migrations.MigrateDatabase();
		}
	}
}
