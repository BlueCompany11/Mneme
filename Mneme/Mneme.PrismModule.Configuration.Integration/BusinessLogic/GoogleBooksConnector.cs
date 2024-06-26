﻿using System;
using System.IO;
using Mneme.Integrations.GoogleBooks.Authorization;

namespace Mneme.PrismModule.Configuration.Integration.BusinessLogic
{
	public class GoogleBooksConnector
	{
		private readonly GoogleBooksService service;

		public GoogleBooksConnector(GoogleBooksService service)
		{
			this.service = service;
		}

		public void Connect()
		{
			service.Connect();
		}

		public bool Disconnect()
		{
			var folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Google.Apis.Auth");

			try
			{
				Directory.Delete(folderPath, true);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
