using System;
using MaterialDesignThemes.Wpf;
using Mneme.Core.Interfaces;
using Mneme.Integrations.Contracts;
using Mneme.Model.Sources;
using Prism.Commands;
using Prism.Mvvm;

namespace Mneme.Desktop.ViewModels
{
	public class MainWindowViewModel : BindableBase
	{
		private readonly IBundledIntegrationFacades integrationFacade;

		public ISnackbarMessageQueue SnackbarMessageQueue { get; }
		public DelegateCommand MigrateDatabaseCommand { get; set; }

		public MainWindowViewModel(IBundledIntegrationFacades integrationFacade, ISnackbarMessageQueue snackbarMessageQueue)
		{
			MigrateDatabaseCommand = new DelegateCommand(MigrateDatabase);
			this.integrationFacade = integrationFacade;
			SnackbarMessageQueue = snackbarMessageQueue;
		}

		private async void MigrateDatabase()
		{
			await integrationFacade.MigrateDatabase();
		}
	}
}
