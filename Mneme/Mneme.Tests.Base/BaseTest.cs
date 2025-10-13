using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using Mneme.Model;
using System.Collections.Immutable;
namespace Mneme.Tests.Base;
public abstract class BaseTest
{
	private readonly Random random;
	protected IFixture fixture;
	public BaseTest()
	{
		random = new();
		fixture = new Fixture().Customize(new AutoMoqCustomization());
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
	protected IReadOnlyList<T> CreateMany<T>() => fixture.CreateMany<T>(random.Next(2, 30)).ToImmutableList();
}
