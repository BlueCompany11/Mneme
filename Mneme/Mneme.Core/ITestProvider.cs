using Mneme.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mneme.Core;

public interface ITestProvider
{
	Task<IReadOnlyList<ITest>> GetAllTests(CancellationToken ct);
	Task<IReadOnlyList<ITest>> GetTestsForToday(CancellationToken ct);
}
