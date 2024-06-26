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
		public async Task MigrateDatabase(CancellationToken ct)
		{
			using var context = new TestingContext();
			await context.Database.MigrateAsync(ct);
		}
	}
}
