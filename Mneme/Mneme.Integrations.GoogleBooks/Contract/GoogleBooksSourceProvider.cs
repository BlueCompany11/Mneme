using Mneme.Integrations.Contracts;
using Mneme.Integrations.GoogleBooks.Authorization;
using Mneme.Integrations.GoogleBooks.Database;

namespace Mneme.Integrations.GoogleBooks.Contract;

public class GoogleBooksSourceProvider : BaseSourcesProvider<GoogleBooksSource>
{
	private readonly GoogleBooksService googleBooksService;

	public GoogleBooksSourceProvider(GoogleBooksService googleBooksService) => this.googleBooksService = googleBooksService;

	protected override void AddSources(List<GoogleBooksSource> sources)
	{
		using var context = new GoogleBooksContext();
		context.AddRange(sources);
		_ = context.SaveChanges();
	}

	protected override async Task<List<GoogleBooksSource>> GetSourcesFromAccountAsync(CancellationToken ct)
	{
		var ret = new List<GoogleBooksSource>();
		try
		{
			googleBooksService.Connect();
		} catch (FileNotFoundException)
		{
			return ret;
		}
		var annotations = await googleBooksService.LoadNotes(ct).ConfigureAwait(false);
		annotations = annotations.GroupBy(x => x.Source.IntegrationId).Select(x => x.First()).ToList();
		foreach (var item in annotations)
		{
			ret.Add(new GoogleBooksSource { Title = item.Source.Title, GoogleBooksSourceId = item.Source.IntegrationId, Active = true });
		}
		return ret;
	}

	protected override List<GoogleBooksSource> GetSourcesFromDatabase()
	{
		var ret = new List<GoogleBooksSource>();
		using (var context = new GoogleBooksContext())
			ret.AddRange(context.GoogleBooksSources);
		return ret;
	}
}
