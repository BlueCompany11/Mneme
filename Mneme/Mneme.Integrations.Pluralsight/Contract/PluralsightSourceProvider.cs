using Mneme.Integrations.Contracts;
using Mneme.Integrations.Pluralsight.Database;

namespace Mneme.Integrations.Pluralsight.Contract
{
	public class PluralsightSourceProvider : BaseSourcesProvider<PluralsightSource>
	{
		private readonly PluralsightNoteProviderDecorator pluralsightNoteProvider;

		public PluralsightSourceProvider(PluralsightNoteProviderDecorator pluralsightNoteProvider)
		{
			this.pluralsightNoteProvider = pluralsightNoteProvider;
		}

		protected override void AddSources(List<PluralsightSource> sources)
		{
			using var pluralsightContext = new PluralsightContext();
			pluralsightContext.PluralsightSources.AddRange(sources);
		}

		protected async override Task<List<PluralsightSource>> GetSourcesFromAccountAsync(CancellationToken ct)
		{
			var notes = await pluralsightNoteProvider.GetNotesAsync(ct);
			notes = notes.GroupBy(x => x.Title).Select(x => x.First()).ToList();
			var ret = new List<PluralsightSource>();
			foreach (PluralsightNote item in notes)
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
