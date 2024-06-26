using System.Collections.Generic;
using System.Threading.Tasks;
using Mneme.Integrations.Contracts;

namespace Mneme.PrismModule.Integration.Facade
{
	public class DatabaseMigrations
	{
		private readonly IEnumerable<IDatabase> databases;

		public DatabaseMigrations(IEnumerable<IDatabase> databases)
		{
			this.databases = databases;
		}
		public async Task MigrateDatabase()
		{
			foreach (var db in databases)
			{
				await db.MigrateDatabase();
			}
		}
	}
}
