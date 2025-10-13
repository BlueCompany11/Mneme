using Mneme.Model;

namespace Mneme.Tests.Base;
public class MockedTest : Test
{
	public override string GetAnswer() => throw new NotImplementedException();
	public override string GetHint() => throw new NotImplementedException();
}
