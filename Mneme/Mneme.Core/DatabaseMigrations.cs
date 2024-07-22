using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mneme.Core;

public class DatabaseMigrations : IDatabaseMigrations
{
	private readonly IEnumerable<IDatabase> databases;
	private readonly SemaphoreSlim semaphore = new(1, 1);
	private bool isMigrated = false;

	public DatabaseMigrations(IEnumerable<IDatabase> databases) => this.databases = databases;

	public async Task MigrateDatabases()
	{
		if (!isMigrated)
		{
			await semaphore.WaitAsync();
			try
			{
				if (!isMigrated)
				{
					foreach (IDatabase db in databases)
					{
						await db.MigrateDatabase();
					}
					isMigrated = true;
				}
			}
			finally
			{
				_ = semaphore.Release();
			}
		}
	}
}
