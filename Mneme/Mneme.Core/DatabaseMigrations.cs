using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mneme.Core
{
	public class DatabaseMigrations : IDatabaseMigrations
	{
		private readonly IEnumerable<IDatabase> databases;
		private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
		private bool isMigrated = false;

		public DatabaseMigrations(IEnumerable<IDatabase> databases)
		{
			this.databases = databases;
		}

		public async Task MigrateDatabases()
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
