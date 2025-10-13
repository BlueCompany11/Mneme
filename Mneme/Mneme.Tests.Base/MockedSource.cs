using Mneme.Model;

namespace Mneme.Tests.Base;
public class MockedSource : Source
{
	public override string TextType => throw new NotImplementedException();

	public override string GetDetails() => throw new NotImplementedException();
}

