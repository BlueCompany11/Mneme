using Mneme.Integrations.Pluralsight.Database;

namespace Mneme.Integrations.Pluralsight
{
	public class PluralsightConfigProvider
	{
		public PluralsightConfig Config { get; private set; }
		public PluralsightConfigProvider()
		{
			using var pluralsightContext = new PluralsightContext();
			try
			{
				Config = pluralsightContext.PluralsightConfigs.SingleOrDefault();
				if (Config == null)
					Config = new();
			}
			catch (Microsoft.Data.Sqlite.SqliteException)
			{
				Config = new();
			}
		}

		public void UpdatePath(string path)
		{
			Config.FilePath = path;
			using var pluralsightContext = new PluralsightContext();
			pluralsightContext.Attach(Config);
			pluralsightContext.SaveChanges();
		}
	}
}
