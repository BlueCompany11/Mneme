namespace Mneme.Integrations.Contracts
{
	public interface IDatabase
	{
		Task MigrateDatabase(CancellationToken ct = default);
	}
}
