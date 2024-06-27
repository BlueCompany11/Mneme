using System;
using MaterialDesignThemes.Wpf;
using Mneme.Integrations.GoogleBooks.Authorization;
using Prism.Commands;
using Prism.Events;

namespace Mneme.PrismModule.Configuration.Integration.ViewModels
{
	public class GoogleBooksSourceConfigurationViewModel : SourceConfigurationEntryBaseViewModel
	{
		private readonly GoogleBooksService service;
		private readonly ISnackbarMessageQueue queue;

		public string Format1 { get; set; }
		public string Format2 { get; set; }
		public string Format3 { get; set; }
		public string Format4 { get; set; }
		public bool IsButtonEnabled { get; set; } = true;
		public string ToolTip { get; set; } = "";

		public DelegateCommand ConnectCommand { get; set; }

		public GoogleBooksSourceConfigurationViewModel(IEventAggregator eventAggregator, GoogleBooksService service, ISnackbarMessageQueue queue) : base(eventAggregator)
		{
			SourceName = "Google Books";
			Format1 = ".epub";
			Format2 = ".pdf";
			this.service = service;
			this.queue = queue;
			ConnectCommand = new DelegateCommand(Connect);
		}

		private void Connect()
		{
			try
			{
				service.Connect();
			}
			catch(Exception)
			{
				queue.Enqueue("Failed to connect to Google Books");
			}
		}
	}
}
