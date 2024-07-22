using System.Threading.Tasks;

namespace Mneme.Core;

public interface IDatabaseMigrations
{
	Task MigrateDatabases();
}
