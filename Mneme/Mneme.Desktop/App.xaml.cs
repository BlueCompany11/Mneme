using DryIoc;
using MaterialDesignThemes.Wpf;
using Mneme.Core;
using Mneme.Desktop.Views;
using Mneme.PrismModule.Configuration.Integration;
using Mneme.PrismModule.Dashboard;
using Mneme.PrismModule.Dashboard.Views;
using Mneme.PrismModule.Integration.Facade;
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
using System;
using System.Windows;

namespace Mneme.Desktop;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
	protected override Window CreateShell() => Container.Resolve<MainWindow>();

	protected override void RegisterTypes(IContainerRegistry containerRegistry) => containerRegistry.RegisterSingleton<ISnackbarMessageQueue>(() => new SnackbarMessageQueue(TimeSpan.FromSeconds(4)));

	protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) => moduleCatalog
			.AddModule<DashboardModule>()
			.AddModule<SourcesModule>()
			.AddModule<NotesModule>()
			.AddModule<GoogleBooksModule>()
			.AddModule<MnemeModule>()
			.AddModule<PluralsightModule>()
			.AddModule<TestingModule>()
			.AddModule<IntegrationModule>()
			.AddModule<BaseModule>()
			.AddModule<IntegrationFacadeModule>();

	protected override void OnInitialized()
	{
		var migrations = Container.Resolve<IDatabaseMigrations>();
		migrations.MigrateDatabases().GetAwaiter().GetResult();
		_ = Container.Resolve<IRegionManager>()
			.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(DashboardView))
			.RegisterViewWithRegion(RegionNames.NoteRegion, typeof(EmptyNotePreviewView))
			.RegisterViewWithRegion(RegionNames.SideBarMenuRegion, typeof(MainMenuSideBarView));
		base.OnInitialized();
	}
}
