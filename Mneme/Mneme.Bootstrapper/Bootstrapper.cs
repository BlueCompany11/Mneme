using System.Collections.Generic;
using DryIoc;
using Mneme.Core;
using Mneme.Core.Google;
using Mneme.Core.Google.GoogleBooks;
using Mneme.Core.Google.GoogleBooks.Helpers;
using Mneme.Core.HardSources;
using Mneme.Core.Pluralsight;
using Mneme.Core.PreelaborationsProviders;
using Mneme.Core.SourceProviders;
using Mneme.Core.TestCreation;
using Mneme.Core.UsersTests;
using Mneme.Core.Visitors;
using Mneme.DataAccess;
using Mneme.Model.Interfaces;
using Mneme.Model.RepetitionAlgorithm;
using Mneme.Model.Sources;

namespace Mneme.Bootstrapper
{
	public class Bootstrapper
	{
		public Container Container { get; private set; }
		public Bootstrapper()
		{
			this.Container = new();
			this.Container.RegisterInstance(new SourcesContext());

			TestVisitors();
			UsersTests();
			Sources();
			HardSources();
			GoogleHelpers();
			PluralsightHelper();
			PreelaborationVisitors();
			PreelaborationProviders();
			DataAccess();
		}


		void GoogleHelpers()
		{
			this.Container.Register<IGoogleCredentialsProvider, GoogleCredentialsProvider>();
			this.Container.Register<GoogleBooksService>();
			this.Container.Register<GoogleBooksAnnotationToPreelaborationMapper>();
		}
		void PluralsightHelper()
		{
			this.Container.RegisterInstance(new PluralsightConfigProvider(this.Container.Resolve<SourcesContext>()));
			this.Container.Register<PluralsightNoteIdProvider>();
		}
		void HardSources()
		{
			this.Container.Register<ISourceSaver<BookSource>, BookSourceSaver>();
			this.Container.Register<ISourceSaver<MiscellaneousSource>, MiscellaneousSourceSaver>();
			this.Container.Register<ISourceSaver<WWWSource>, WWWSourceSaver>();
			this.Container.Register<ISourceSaver<MagazineSource>, MagazineSourceSaver>();
		}
		void PreelaborationProviders()
		{
			this.Container.Register<IPreelaborationProviderMother, PreelaborationsProviderMother>();
			this.Container.Register<IPreelaborationsProvider, PluralsightPreelaborationProviderDecorator>();
			this.Container.Register<IPreelaborationsProvider, GoogleBooksPreelaborationProvider>();
			this.Container.Register<IPluralsightPreelaborationProvider, PluralsightPreelaborationProvider>();
			this.Container.RegisterInstance(new List<IPreelaborationsProvider>(Container.ResolveMany<IPreelaborationsProvider>()));
		}
		void PreelaborationVisitors()
		{
			this.Container.Register<IPreelaborationPreviewVisitor, PreelaborationPreviewVisitor>();
			this.Container.Register<INotePreviewVisitor, NotePreviewVisitor>();
		}
		void TestVisitors()
		{
			this.Container.RegisterInstance(new ClozeDeletionNoteTestVisitor());
			this.Container.RegisterInstance(new MultipleChoiceNoteTestVisitor());
			this.Container.RegisterInstance(new ShortAnswerNoteTestVisitor());
		}
		void UsersTests()
		{
			this.Container.Register<TestPreviewProvider>();
			this.Container.RegisterInstance(new TestTypeProvider());
			this.Container.RegisterInstance(new ClozeTestTextHelper());
			this.Container.RegisterInstance(new TestImportanceMapper());
			this.Container.RegisterInstance(new Fibo());
			this.Container.Register<SpaceRepetition>();
		}
		void Sources()
		{
			this.Container.Register<ISourcePreviewProvider, SourcesProvider>();
			this.Container.Register<GoogleBooksSourceProvider>();
			this.Container.Register<PluralsightSourceProvider>();
			this.Container.Register<IBundledSourceProviders, BundledSourceProviders>();
		}
		void DataAccess()
		{
			this.Container.Register<DataAccessProvider>();
		}
	}
}
