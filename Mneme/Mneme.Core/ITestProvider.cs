using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mneme.Model;

namespace Mneme.Core
{
	public interface ITestProvider
	{
		Task<IReadOnlyList<Test>> GetAllTests();
		Task<IReadOnlyList<Test>> GetTestsForToday();
	}
}
