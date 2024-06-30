using System;
using MaterialDesignThemes.Wpf;
using Mneme.Integrations.GoogleBooks.Authorization;
using Mneme.PrismModule.Configuration.Integration.BusinessLogic;
using Prism.Commands;
using Prism.Events;

namespace Mneme.PrismModule.Configuration.Integration.ViewModels
{
	public class GoogleBooksSourceConfigurationViewModel : SourceConfigurationEntryBaseViewModel
	{
		private readonly GoogleBooksConnector connector;
		private readonly ISnackbarMessageQueue queue;

		public string Format1 { get; set; }
		public string Format2 { get; set; }
		public string Format3 { get; set; }
		public string Format4 { get; set; }
		public bool IsButtonEnabled { get; set; } = true;
		public string ToolTip { get; set; } = "";
		private string status;

		public string Status
		{
			get => status;
			set => SetProperty(ref status, value);
		}
		
		public DelegateCommand ConnectCommand { get; set; }
		public DelegateCommand DisconnectCommand { get; set; }

		public GoogleBooksSourceConfigurationViewModel(IEventAggregator eventAggregator, GoogleBooksConnector connector,  ISnackbarMessageQueue queue) : base(eventAggregator)
		{
			SourceName = "Google Books";
			Format1 = ".epub";
			Format2 = ".pdf";
			this.connector = connector;
			this.queue = queue;
			ConnectCommand = new DelegateCommand(Connect);
			DisconnectCommand = new DelegateCommand(Disconnect);
			Status = "Unknown";
		}

		private void Connect()
		{
			try
			{
				connector.Connect();
				queue.Enqueue("Connection with Google Books account established.");
				Status = "Connected";
			}
			catch(Exception)
			{
				Status = "Unable to connect";
				queue.Enqueue("Failed to connect to Google Books.");
			}
		}

		private void Disconnect()
		{
			if (connector.Disconnect())
			{
				Status = "Disconnected";
				queue.Enqueue("Disconnected from Google Books account. All sources and notes will remain in Mneme.");
			}
			else
			{
				Status = "Unknown";
				queue.Enqueue("Failed to disconnect");
			}

		}
	}
}
