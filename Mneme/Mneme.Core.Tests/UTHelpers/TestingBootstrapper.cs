using AutoFixture;

namespace Mneme.Core.Tests.UTHelpers
{
	public class TestingBootstrapper
	{
		private readonly Fixture fixture;
		private readonly DryIoc.Container container;
		public TestingBootstrapper()
		{
			fixture = new Fixture();

			var contextFactory = new InMemoryDataContextFactory();
			var context = contextFactory.Build(fixture.Create<string>());

			//var test = bootstrapper.Container.Resolve<SourcesContext>();
			//bootstrapper.Container.RegisterInstance(context, IfAlreadyRegistered.Replace);
			//var test2 = bootstrapper.Container.Resolve<SourcesContext>();
			var bootstrapper = new Core.Bootstrapper.Bootstrapper(context);
			container = bootstrapper.Container;
		}
		public DryIoc.Container GetBootstrapper()
		{
			return container;
		}
	}
}
