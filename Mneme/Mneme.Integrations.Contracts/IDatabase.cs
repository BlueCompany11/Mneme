namespace Mneme.Integrations.Contracts
{
	public interface IDatabase : IDisposable
	{
		Task MigrateDatabase(CancellationToken ct);
	}
}
