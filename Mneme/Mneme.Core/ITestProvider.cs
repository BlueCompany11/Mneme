using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mneme.Core
{
	public interface ITestProvider
	{
		int GetAllTestsCount();
		int GetTodaysTests();
	}
}
