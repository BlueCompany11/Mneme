using System.Threading.Tasks;
using FluentAssertions;
using Mneme.Core.Tests.UTHelpers;
using Mneme.Integrations.Pluralsight;
using Xunit;

namespace Mneme.Core.Tests.PreelaborationsProviders
{
	public class PluralsightPreelaborationProviderDecoratorShould
	{
		private readonly PluralsightPreelaborationProviderDecorator sut;
		private readonly PluralsightPreelaborationProviderDecoratorFactory providerFactory;

		public PluralsightPreelaborationProviderDecoratorShould()
		{
			providerFactory = new();
			sut = providerFactory.Build();
		}

		[Fact]
		public async Task ReturnEmptyListOfPreelaborationsWhenNoFile()
		{
			var notes = await sut.GetPreelaborationsAsync();
			notes.Should().BeEmpty();
		}

		[Fact]
		public async Task ReturnNonEmptyListWhenSourceIsCorrectlyUpdated()
		{
			providerFactory.PluralsightConfigProviderArg.UpdatePath("user-notes.csv");
			var notes = await sut.GetPreelaborationsAsync();
			notes.Should().NotBeEmpty();
		}

		[Fact]
		public void TriggerEventIfSourceIsUpdated()
		{
			using var monitoredSut = sut.Monitor();
			providerFactory.PluralsightConfigProviderArg.UpdatePath("user-notes.csv");
			_ = monitoredSut.Should().Raise(nameof(sut.PreelaborationsUpdated));
		}
	}
}
