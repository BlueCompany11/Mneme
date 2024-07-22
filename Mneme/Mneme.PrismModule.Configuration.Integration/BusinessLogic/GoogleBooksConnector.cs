using Mneme.Integrations.GoogleBooks.Authorization;
using System;
using System.IO;

namespace Mneme.PrismModule.Configuration.Integration.BusinessLogic;

public class GoogleBooksConnector : IDisposable
{
	private readonly GoogleBooksService service;

	public GoogleBooksConnector(GoogleBooksService service) => this.service = service;

	public void Connect() => service.Connect();

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

	public void Dispose() => service.Dispose();
}
