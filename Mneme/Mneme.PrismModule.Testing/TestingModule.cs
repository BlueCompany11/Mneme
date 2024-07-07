using Mneme.Integrations.Contracts;
using Mneme.PrismModule.Testing.Views.TestCreation;
using Mneme.PrismModule.Testing.Views.UsersTests;
using Mneme.Testing.Contracts;
using Mneme.Testing.RepetitionAlgorithm;
using Mneme.Testing.TestCreation;
using Mneme.Testing.UsersTests;
using Prism.Ioc;
using Prism.Modularity;

namespace Mneme.PrismModule.Testing
{
	public class TestingModule : IModule
	{
		public void OnInitialized(IContainerProvider containerProvider)
		{

		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<TestCreationView>();
			containerRegistry.RegisterForNavigation<ShortAnswerTestCreationView>();
			containerRegistry.RegisterForNavigation<MultipleChoiceTestCreationView>();
			containerRegistry.RegisterForNavigation<ClozeDeletionTestCreationView>();

			containerRegistry.RegisterForNavigation<TestsView>();
			containerRegistry.RegisterForNavigation<ShortAnswerTestView>();
			containerRegistry.RegisterForNavigation<MultipleAnswersTestView>();
			containerRegistry.RegisterForNavigation<ClozeDeletionTestView>();
			containerRegistry.RegisterForNavigation<TestingView>();

			containerRegistry.Register<TestPreviewProvider>();
			containerRegistry.Register<TestTypeProvider>();
			containerRegistry.Register<TestImportanceMapper>();
			containerRegistry.Register<Fibo>();
			containerRegistry.Register<SpaceRepetition>();

			containerRegistry.Register<IDatabase, DatabaseMigrator>();
			containerRegistry.Register<TestingRepository>();
		}
	}
}