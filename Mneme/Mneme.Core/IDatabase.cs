using System.Threading;
using System.Threading.Tasks;

namespace Mneme.Core;

public interface IDatabase
{
	Task MigrateDatabase(CancellationToken ct = default);
}
