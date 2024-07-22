using AutoFixture;
using AutoFixture.Kernel;
using Mneme.Model;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mneme.Tests.Base;
public abstract class BaseTest
{
	protected Fixture fixture;
	public BaseTest()
	{
		fixture = new();
		fixture.Customizations.Add(
		new TypeRelay(
		typeof(Source),
		typeof(MockedSource)));
		fixture.Customizations.Add(
		new TypeRelay(
		typeof(Note),
		typeof(MockedNote)));
		fixture.Customizations.Add(
		new TypeRelay(
		typeof(Test),
		typeof(MockedTest)));
	}
    protected IReadOnlyList<T> CreateMany<T>() => fixture.CreateMany<T>(fixture.Create<int>()).ToImmutableList();
}
