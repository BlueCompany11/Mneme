using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DryIoc;
using FluentAssertions;
using Xunit;

namespace Mneme.Bootstrapper.Tests
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
			obj1.Should().BeSameAs(obj2);
			var sut = new RegistrationTransferer();
			sut.TransferRegistrations(c1, c2);
			var obj3 = c2.Resolve<RegistrationTransferer>();
			var obj4 = c2.Resolve<RegistrationTransferer>();
			obj3.Should().BeSameAs(obj4);
		}
	}
}
