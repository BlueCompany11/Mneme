using Mneme.Integrations.Contracts;
using Mneme.Integrations.Pluralsight.Database;

namespace Mneme.Integrations.Pluralsight.Contract
{
	public class PluralsightSourceProvider : BaseSourcesProvider<PluralsightSource>
	{
		private readonly PluralsightPreelaborationProviderDecorator pluralsightPreelaborationProvider;

		public PluralsightSourceProvider(PluralsightPreelaborationProviderDecorator pluralsightPreelaborationProvider)
		{
			this.pluralsightPreelaborationProvider = pluralsightPreelaborationProvider;
		}

		protected override void AddSources(List<PluralsightSource> sources)
		{
			using var pluralsightContext = new PluralsightContext();
			pluralsightContext.PluralsightSources.AddRange(sources);
		}

		protected async override Task<List<PluralsightSource>> GetSourcesFromAccountAsync(CancellationToken ct)
		{
			var preelaborations = await pluralsightPreelaborationProvider.GetPreelaborationsAsync(ct);
			preelaborations = preelaborations.GroupBy(x => x.Title).Select(x => x.First()).ToList();
			var ret = new List<PluralsightSource>();
			foreach (PluralsightPreelaboration item in preelaborations)
			{
				ret.Add(new PluralsightSource(item) { Active = true, IntegrationId = item.Source.IntegrationId, Title = item.Source.Title });
			}
			return ret;
		}

		protected override List<PluralsightSource> GetSourcesFromDatabase()
		{
			using var pluralsightContext = new PluralsightContext();
			return pluralsightContext.PluralsightSources.ToList();
		}

		protected override bool AreSame(PluralsightSource note1, PluralsightSource note2)
		{
			return note1.Title == note2.Title;
		}
	}
}
