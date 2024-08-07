﻿using DryIoc;
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

	protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
	{
		_ = moduleCatalog.AddModule<DashboardModule>(InitializationMode.WhenAvailable);
		_ = moduleCatalog.AddModule<SourcesModule>(InitializationMode.WhenAvailable);
		_ = moduleCatalog.AddModule<NotesModule>(InitializationMode.WhenAvailable);
		_ = moduleCatalog.AddModule<GoogleBooksModule>(InitializationMode.WhenAvailable);
		_ = moduleCatalog.AddModule<MnemeModule>(InitializationMode.WhenAvailable);
		_ = moduleCatalog.AddModule<PluralsightModule>(InitializationMode.WhenAvailable);
		_ = moduleCatalog.AddModule<TestingModule>(InitializationMode.WhenAvailable);
		_ = moduleCatalog.AddModule<IntegrationModule>(InitializationMode.WhenAvailable);
		_ = moduleCatalog.AddModule<BaseModule>(InitializationMode.WhenAvailable);
		_ = moduleCatalog.AddModule<IntegrationFacadeModule>(InitializationMode.WhenAvailable);
	}

	protected override void OnInitialized()
	{
		IRegionManager regionManager = Container.Resolve<IRegionManager>();
		_ = regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(DashboardView));
		_ = regionManager.RegisterViewWithRegion(RegionNames.NoteRegion, typeof(EmptyNotePreviewView));
		_ = regionManager.RegisterViewWithRegion(RegionNames.SideBarMenuRegion, typeof(MainMenuSideBarView));
		base.OnInitialized();
	}

	protected override async void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);
		IDatabaseMigrations migrations = Container.Resolve<IDatabaseMigrations>();
		await migrations.MigrateDatabases();
	}
}
