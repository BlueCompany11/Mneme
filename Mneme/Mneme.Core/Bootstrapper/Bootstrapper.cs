using System.ComponentModel;
using DryIoc;
using Mneme.Core.Interfaces;
using Mneme.DataAccess;
using Mneme.Integrations.Contracts;
using Mneme.Integrations.GoogleBooks.Authorization;
using Mneme.Integrations.GoogleBooks.Contract;
using Mneme.Integrations.GoogleBooks.Database;
using Mneme.Integrations.Mneme;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Integrations.Mneme.Database;
using Mneme.Integrations.Pluralsight;
using Mneme.Integrations.Pluralsight.Contract;
using Mneme.Integrations.Pluralsight.Database;
using Mneme.Model.Interfaces;
using Mneme.Testing.Database;
using Mneme.Testing.RepetitionAlgorithm;
using Mneme.Testing.TestCreation;
using Mneme.Testing.UsersTests;

namespace Mneme.Core.Bootstrapper
{
	public class Bootstrapper
	{
		public DryIoc.Container Container { get; private set; }
		public Bootstrapper()
		{
			Container = new(rules => rules.WithTrackingDisposableTransients());

			Sources();
			HardSources();
			GoogleHelpers();
			PluralsightHelper();
			PreelaborationProviders();
			Integrations();
		}

		private void GoogleHelpers()
		{
			Container.Register<GoogleCredentialsProvider>();
			Container.Register<GoogleBooksService>();
		}

		private void PluralsightHelper()
		{
			Container.RegisterInstance(new PluralsightConfigProvider());
			Container.Register<PluralsightNoteIdProvider>();
		}

		private void HardSources()
		{
			Container.Register<ISourceSaver<MnemeSource>, MnemeSourceSaver>();
			Container.Register<MnemeNoteSaver>();
		}

		private void PreelaborationProviders()
		{
			Container.Register<PluralsightPreelaborationProviderDecorator>();
			Container.Register<GoogleBooksPreelaborationProvider>();
			Container.Register<IPluralsightPreelaborationProvider, PluralsightPreelaborationProvider>();
		}

		private void Sources()
		{
			Container.Register<BaseSourcesProvider<GoogleBooksSource>, GoogleBooksSourceProvider>();
			Container.Register<BaseSourcesProvider<PluralsightSource>, PluralsightSourceProvider>();
			Container.Register<BaseSourcesProvider<MnemeSource>, MnemeSourcesProvider>();
		}

		private void Integrations()
		{
			Container.Register<IIntegrationFacade<GoogleBooksSource, GoogleBooksPreelaboration>, GoogleBooksIntegrationFacade>();
			Container.Register<IIntegrationFacade<MnemeSource, MnemePreelaboration>, MnemeIntegrationFacade>();
			Container.Register<IIntegrationFacade<PluralsightSource, PluralsightPreelaboration>, PluralsightIntegrationFacade>();
			Container.Register<IBundledIntegrationFacades, BundledIntegrationFacades>();
		}
	}
}
