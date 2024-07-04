using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mneme.Integrations.Contracts;

namespace Mneme.PrismModule.Integration.Facade
{
	public class DatabaseMigrations
	{
		private readonly IEnumerable<IDatabase> databases;
		private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
		private bool isMigrated = false;

		public DatabaseMigrations(IEnumerable<IDatabase> databases)
		{
			this.databases = databases;
		}

		public async Task MigrateDatabase()
		{
			if (!isMigrated)
			{
				await semaphore.WaitAsync();
				try
				{
					if (!isMigrated)
					{
						foreach (var db in databases)
						{
							await db.MigrateDatabase();
						}
						isMigrated = true;
					}
				}
				finally
				{
					semaphore.Release();
				}
			}
		}
	}
}
