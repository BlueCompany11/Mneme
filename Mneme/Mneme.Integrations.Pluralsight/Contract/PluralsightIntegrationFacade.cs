using Mneme.DataAccess;
using Mneme.Integrations.Contracts;
using Mneme.Integrations.Pluralsight.Database;

namespace Mneme.Integrations.Pluralsight.Contract
{
	public class PluralsightIntegrationFacade : IntegrationFacadeBase<Context, PluralsightSource, PluralsightPreelaboration>
	{
		private readonly BaseSourcesProvider<PluralsightSource> pluralsightSourceProvider;
		private readonly PluralsightPreelaborationProviderDecorator noteProvider;

		public PluralsightIntegrationFacade(BaseSourcesProvider<PluralsightSource> pluralsightSourceProvider, PluralsightPreelaborationProviderDecorator noteProvider) : base()
		{
			this.pluralsightSourceProvider = pluralsightSourceProvider;
			this.noteProvider = noteProvider;
		}

		protected override Context CreateContext()
		{
			return new PluralsightContext();
		}
		public override async Task<IReadOnlyList<PluralsightPreelaboration>> GetNotes(CancellationToken ct)
		{
			var ret = new List<PluralsightPreelaboration>();
			foreach (var note in await noteProvider.GetPreelaborationsAsync(ct))
			{
				ret.Add((PluralsightPreelaboration)note);
			}
			return ret;
		}
		public override async Task<IReadOnlyList<PluralsightSource>> GetSources(CancellationToken ct)
		{
			var ret = new List<PluralsightSource>();
			foreach (var note in await pluralsightSourceProvider.GetSourcesAsync(false, ct))
			{
				ret.Add((PluralsightSource)note);
			}
			return ret;
		}
	}
}
