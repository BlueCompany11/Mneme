using Mneme.Integrations.Contracts;
using Mneme.Integrations.Pluralsight.Database;

namespace Mneme.Integrations.Pluralsight.Contract;

public class PluralsightSourceProvider : BaseSourcesProvider<PluralsightSource>
{
	private readonly PluralsightNoteProviderDecorator pluralsightNoteProvider;

	public PluralsightSourceProvider(PluralsightNoteProviderDecorator pluralsightNoteProvider) => this.pluralsightNoteProvider = pluralsightNoteProvider;

	protected override void AddSources(List<PluralsightSource> sources)
	{
		using var pluralsightContext = new PluralsightContext();
		pluralsightContext.PluralsightSources.AddRange(sources);
	}

	protected override async Task<List<PluralsightSource>> GetSourcesFromAccountAsync(CancellationToken ct)
	{
		List<Model.Note> notes = await pluralsightNoteProvider.GetNotesAsync(ct).ConfigureAwait(false);
		notes = notes.GroupBy(x => x.Title).Select(x => x.First()).ToList();
		var ret = new List<PluralsightSource>();
		foreach (PluralsightNote item in notes)
		{
			ret.Add(new PluralsightSource(item) { Active = true, PluralsightSourceId = item.Source.IntegrationId, Title = item.Source.Title });
		}
		return ret;
	}

	protected override List<PluralsightSource> GetSourcesFromDatabase()
	{
		using var pluralsightContext = new PluralsightContext();
		return pluralsightContext.PluralsightSources.ToList();
	}
}
