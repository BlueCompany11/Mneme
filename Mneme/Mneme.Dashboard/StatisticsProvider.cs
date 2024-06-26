using System;
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

		public async Task<int> GetActiveSourcesCount(CancellationToken ct = default) => (await integration.GetActiveSources(ct)).Count;
		public async Task<int> GetActiveNotesCount(CancellationToken ct = default) => (await integration.GetActiveNotes(ct)).Count;
		public async Task<string?> GetMostRecentSource(CancellationToken ct = default) => (await integration.GetActiveSources(ct)).OrderBy(x => x.Created).Select(x => x.Title).FirstOrDefault();
		public async Task<string?> GetMostRecentNote(CancellationToken ct = default) => (await integration.GetActiveNotes(ct)).OrderBy(x => x.CreationTime).Select(x => x.Title + Environment.NewLine + x.Content).FirstOrDefault();

		public void Dispose()
		{
			integration.Dispose();
		}
	}
}
