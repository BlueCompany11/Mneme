using Mneme.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mneme.Core;

public interface ITestProvider
{
	Task<IReadOnlyList<Test>> GetAllTests();
	Task<IReadOnlyList<Test>> GetTestsForToday();
}
