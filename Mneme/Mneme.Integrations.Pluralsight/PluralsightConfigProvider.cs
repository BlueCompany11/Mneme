using Mneme.Integrations.Pluralsight.Database;

namespace Mneme.Integrations.Pluralsight;

public class PluralsightConfigProvider
{
	public PluralsightConfig Config { get; private set; }
	public PluralsightConfigProvider()
	{
		using var pluralsightContext = new PluralsightContext();
		try
		{
			Config = pluralsightContext.PluralsightConfigs.SingleOrDefault();
			Config ??= new();
		} catch (Microsoft.Data.Sqlite.SqliteException)
		{
			Config = new();
		}
	}

	public void UpdatePath(string path)
	{
		if (path is null)
			throw new ArgumentNullException(nameof(path), "Path cannot be null.");
		Config.FilePath = path;
		using var pluralsightContext = new PluralsightContext();
		_ = pluralsightContext.Attach(Config);
		_ = pluralsightContext.SaveChanges();
	}
}
