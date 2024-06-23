using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mneme.Integrations.Contracts;
using Mneme.Testing.Database;

namespace Mneme.Testing.Contracts
{
	public class DatabaseMigrator : IDatabase
	{
		TestingContext context;
		public DatabaseMigrator()
		{
			context = new TestingContext();
		}
		public void Dispose()
		{
			context.Dispose();
		}

		public async Task MigrateDatabase(CancellationToken ct)
		{
			await context.Database.MigrateAsync(ct);
		}
	}
}
