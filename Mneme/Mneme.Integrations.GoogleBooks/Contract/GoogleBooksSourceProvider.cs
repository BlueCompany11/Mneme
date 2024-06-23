using Mneme.Integrations.Contracts;
using Mneme.Integrations.GoogleBooks.Authorization;
using Mneme.Integrations.GoogleBooks.Database;

namespace Mneme.Integrations.GoogleBooks.Contract
{
	public class GoogleBooksSourceProvider : BaseSourcesProvider<GoogleBooksSource>
	{
		private readonly GoogleBooksService googleBooksService;

		public GoogleBooksSourceProvider(GoogleBooksService googleBooksService)
		{
			this.googleBooksService = googleBooksService;
		}

		protected override void AddSources(List<GoogleBooksSource> sources)
		{
			using var context = new GoogleBooksContext();
			context.AddRange(sources);
			context.SaveChanges();
		}

		protected async override Task<List<GoogleBooksSource>> GetSourcesFromAccountAsync(CancellationToken ct)
		{
			googleBooksService.Connect();
			var annotations = await googleBooksService.LoadNotes(ct);
			annotations = annotations.GroupBy(x => x.Source.IntegrationId).Select(x => x.First()).ToList();
			var ret = new List<GoogleBooksSource>();
			foreach (var item in annotations)
			{
				ret.Add(new GoogleBooksSource { Title = item.Source.Title, IntegrationId = item.Source.IntegrationId, Active = true, });
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

		protected override bool AreSame(GoogleBooksSource note1, GoogleBooksSource note2)
		{
			return note1.IntegrationId == note2.IntegrationId;
		}
	}
}
