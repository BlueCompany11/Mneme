using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace Mneme.Tests.Base;
public class AutoDomainDataAttribute : AutoDataAttribute
{
	public AutoDomainDataAttribute()
	  : base(() => new Fixture().Customize(new AutoMoqCustomization()))
	{
	}
}
