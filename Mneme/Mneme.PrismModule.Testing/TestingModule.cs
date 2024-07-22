using Mneme.Core;
using Mneme.PrismModule.Testing.Views.TestCreation;
using Mneme.PrismModule.Testing.Views.UsersTests;
using Mneme.Testing.Contracts;
using Mneme.Testing.TestCreation;
using Mneme.Testing.UsersTests;
using Prism.Ioc;
using Prism.Modularity;

namespace Mneme.PrismModule.Testing;

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
		containerRegistry.RegisterForNavigation<TestingView>();

		_ = containerRegistry.Register<TestPreviewProvider>();
		_ = containerRegistry.Register<TestTypeProvider>();
		_ = containerRegistry.Register<TestImportanceMapper>();

		_ = containerRegistry.Register<IDatabase, DatabaseMigrator>();
		_ = containerRegistry.Register<TestingRepository>();
		_ = containerRegistry.Register<ITestProvider, TestPreviewProvider>();
	}
}