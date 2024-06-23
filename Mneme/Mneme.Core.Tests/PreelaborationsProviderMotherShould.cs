using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Mneme.Core.PreelaborationsProviders;
using Mneme.Integrations.Pluralsight;
using Mneme.Model.Interfaces;
using Mneme.Model.Preelaborations;
using Moq;
using Xunit;

namespace Mneme.Core.Tests
{
	public class PreelaborationsProviderMotherShould
	{

		[Fact]
		public async Task ProvideCorrectNumberOfPreelaborations()
		{
			var fixture = new Fixture();

			var preelb = fixture.Create<List<PluralsightPreelaboration>>();
			var p = new List<Preelaboration>();
			p.AddRange(preelb);
			var preelaborationProvider1 = new Mock<IPreelaborationsProvider>();
			_ = preelaborationProvider1.Setup(x => x.GetPreelaborationsAsync(CancellationToken.None).Result).Returns(p);


			var preelb2 = fixture.Create<List<GoogleBooksPreelaboration>>();
			var p2 = new List<Preelaboration>();
			p2.AddRange(preelb2);
			var preelaborationProvider2 = new Mock<IPreelaborationsProvider>();
			_ = preelaborationProvider2.Setup(x => x.GetPreelaborationsAsync(CancellationToken.None).Result).Returns(p2);

			var arg1 = new List<IPreelaborationsProvider>
			{
				preelaborationProvider1.Object,
				preelaborationProvider2.Object
			};

			var sut = new PreelaborationsProviderMother(arg1);
			var notes = await sut.GetPreelaborationsAsync();
			notes.Should().HaveCount(preelb.Count + preelb2.Count);
		}
		[Fact]
		public void NotifyIfProviderHasUpdatedSource()
		{
			var fixture = new Fixture();

			var preelb = fixture.Create<List<PluralsightPreelaboration>>();
			var p = new List<Preelaboration>();
			p.AddRange(preelb);
			var preelaborationProvider1 = new Mock<IPreelaborationsProvider>();
			_ = preelaborationProvider1.Setup(x => x.GetPreelaborationsAsync(CancellationToken.None).Result).Returns(p);
			_ = preelaborationProvider1.SetupAdd(x => x.PreelaborationsUpdated += It.IsAny<Action>());

			var arg1 = new List<IPreelaborationsProvider>
			{
				preelaborationProvider1.Object
			};

			var sut = new PreelaborationsProviderMother(arg1);
			using var monitoredSut = sut.Monitor();
			preelaborationProvider1.Raise(x => x.PreelaborationsUpdated += null, new object[0]);
			_ = monitoredSut.Should().Raise(nameof(sut.PreelaborationsUpdated));
		}
	}
}
