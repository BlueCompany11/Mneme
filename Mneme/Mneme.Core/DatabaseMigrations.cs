using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mneme.Core;

public class DatabaseMigrations(IEnumerable<IDatabase> databases) : IDatabaseMigrations
{
	private readonly SemaphoreSlim semaphore = new(1, 1);
	private bool isMigrated = false;

	public async Task MigrateDatabases()
	{
		await semaphore.WaitAsync();
		try
		{
			if (!isMigrated)
			{
				var migrationTasks = new List<Task>();
				foreach (IDatabase db in databases)
				{
					migrationTasks.Add(db.MigrateDatabase());
				}
				await Task.WhenAll(migrationTasks);
				isMigrated = true;
			}
		}
		finally
		{
			_ = semaphore.Release();
		}
	}
}
