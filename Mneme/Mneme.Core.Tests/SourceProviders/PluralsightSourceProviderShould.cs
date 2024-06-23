using System.Collections.Generic;
using System.Threading;
using AutoFixture;
using FluentAssertions;
using Mneme.Core.Tests.UTHelpers;
using Mneme.Integrations.Pluralsight;
using Xunit;

namespace Mneme.Core.Tests.SourceProviders
{
	public class PluralsightSourceProviderShould
	{
		private readonly PluralsightSourceProvider sut;
		private readonly PluralsightPreelaborationProviderDecoratorFactory preelaborationsFactory;
		private readonly Fixture fixture;
		public PluralsightSourceProviderShould()
		{
			preelaborationsFactory = new();
			fixture = new();
			var provider = preelaborationsFactory.Build();
			sut = new PluralsightSourceProvider(provider, preelaborationsFactory.Context);
		}
		[Fact]
		public async void ReturnZeroSourcesOnEmptyDatabase()
		{
			var sources = await sut.GetSourcesAsync(false, CancellationToken.None);
			sources.Should().BeEmpty();
		}

		[Fact]
		public async void ReturnCorrectAmountFromDatabase()
		{
			var sources = fixture.Create<List<PluralsightSource>>();
			preelaborationsFactory.Context.AddRange(sources);
			_ = preelaborationsFactory.Context.SaveChanges();

			var returnedSources = await sut.GetSourcesAsync(false, CancellationToken.None);
			returnedSources.Count.Should().Be(sources.Count);
		}
	}
}
