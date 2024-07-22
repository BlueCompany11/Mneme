using Mneme.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mneme.Tests.Base;
public class MockedTest : Test
{
	public override string GetAnswer() => throw new NotImplementedException();
	public override string GetHint() => throw new NotImplementedException();
}
