using DryIoc;
using FluentAssertions;
using Mneme.Core.Bootstrapper;
using Xunit;

namespace Mneme.Core.Tests.Bootstrapper
{
	public class RegistraionTransfererShould
	{
		[Fact]
		public void CopyParameterIfAlreadyRegistered()
		{
			var c1 = new Container();
			var c2 = new Container();
			c1.Register<RegistrationTransferer>(Reuse.Singleton);
			var obj1 = c1.Resolve<RegistrationTransferer>();
			var obj2 = c1.Resolve<RegistrationTransferer>();
			_ = obj1.Should().BeSameAs(obj2);
			var sut = new RegistrationTransferer();
			sut.TransferRegistrations(c1, c2);
			var obj3 = c2.Resolve<RegistrationTransferer>();
			var obj4 = c2.Resolve<RegistrationTransferer>();
			_ = obj3.Should().BeSameAs(obj4);
		}
	}
}
