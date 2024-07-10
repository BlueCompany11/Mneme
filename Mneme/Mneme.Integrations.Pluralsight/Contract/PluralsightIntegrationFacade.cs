using Mneme.DataAccess;
using Mneme.Integrations.Contracts;
using Mneme.Integrations.Pluralsight.Database;

namespace Mneme.Integrations.Pluralsight.Contract
{
	public class PluralsightIntegrationFacade : IntegrationFacadeBase<Context, PluralsightSource, PluralsightNote>
	{
		private readonly BaseSourcesProvider<PluralsightSource> pluralsightSourceProvider;
		private readonly PluralsightNoteProviderDecorator noteProvider;

		public PluralsightIntegrationFacade(BaseSourcesProvider<PluralsightSource> pluralsightSourceProvider, PluralsightNoteProviderDecorator noteProvider) : base()
		{
			this.pluralsightSourceProvider = pluralsightSourceProvider;
			this.noteProvider = noteProvider;
		}

		protected override Context CreateContext()
		{
			return new PluralsightContext();
		}
		public override async Task<IReadOnlyList<PluralsightNote>> GetNotes(CancellationToken ct)
		{
			var ret = new List<PluralsightNote>();
			foreach (var note in await noteProvider.GetNotesAsync(ct).ConfigureAwait(false))
			{
				ret.Add((PluralsightNote)note);
			}
			return ret;
		}
		public override async Task<IReadOnlyList<PluralsightSource>> GetSources(CancellationToken ct)
		{
			var ret = new List<PluralsightSource>();
			foreach (var note in await pluralsightSourceProvider.GetSourcesAsync(false, ct).ConfigureAwait(false))
			{
				ret.Add((PluralsightSource)note);
			}
			return ret;
		}
	}
}
