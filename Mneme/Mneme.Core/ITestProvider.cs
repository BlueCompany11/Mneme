using Mneme.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mneme.Core;

public interface ITestProvider
{
	Task<IReadOnlyList<Test>> GetAllTests(CancellationToken ct);
	Task<IReadOnlyList<Test>> GetTestsForToday(CancellationToken ct);
}
