using Mneme.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mneme.Tests.Base;
public class MockedSource : Source
{
	public override string TextType => throw new NotImplementedException();

	public override string GetDetails() => throw new NotImplementedException();
}

