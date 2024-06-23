using AutoFixture;
using Mneme.Core.PreelaborationsProviders;
using Mneme.DataAccess;
using Mneme.Integrations.Pluralsight;

namespace Mneme.Core.Tests.UTHelpers
{
	internal class PluralsightPreelaborationProviderDecoratorFactory
	{
		private PluralsightPreelaborationProviderDecorator sut;
		private Fixture fixture;
		public Context Context { get; set; }

		private InMemoryDataContextFactory inMemoryDataContextFactory;
		public PluralsightPreelaborationProvider PluralsightPreelaborationProviderArg { get; set; }
		public PluralsightConfigProvider PluralsightConfigProviderArg { get; set; }
		public PluralsightPreelaborationProviderDecorator Build()
		{
			inMemoryDataContextFactory = new InMemoryDataContextFactory();
			fixture = new();
			Context = inMemoryDataContextFactory.Build(fixture.Create<string>());
			var noteIdProvider = new PluralsightNoteIdProvider();
			PluralsightPreelaborationProviderArg = new PluralsightPreelaborationProvider(noteIdProvider);
			PluralsightConfigProviderArg = new PluralsightConfigProvider(Context);
			sut = new PluralsightPreelaborationProviderDecorator(PluralsightPreelaborationProviderArg, PluralsightConfigProviderArg, Context);
			return sut;
		}
	}
}
