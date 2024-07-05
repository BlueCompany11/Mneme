using Mneme.Core;

namespace Mneme.Dashboard
{
	public class StatisticsProvider : IDisposable
	{
		private readonly IBundledIntegrationFacades integration;

		public StatisticsProvider(IBundledIntegrationFacades integration)
		{
			this.integration = integration;
		}

		public async Task<int> GetKnownSourcesCount(CancellationToken ct = default) => (await integration.GetKnownSources()).Count;
		public async Task<int> GetKnownNotesCount(CancellationToken ct = default) => (await integration.GetKnownNotes()).Count;
		public async Task<string?> GetMostRecentSource(CancellationToken ct = default) => (await integration.GetKnownSources()).OrderBy(x => x.Created).Select(x => x.Title).FirstOrDefault();
		public async Task<string?> GetMostRecentNote(CancellationToken ct = default) => (await integration.GetKnownNotes()).OrderBy(x => x.CreationTime).Select(x => x.Title + Environment.NewLine + x.Content).FirstOrDefault();

		public void Dispose()
		{
			integration.Dispose();
		}
	}
}
