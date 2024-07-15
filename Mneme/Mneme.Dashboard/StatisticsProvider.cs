using Mneme.Core;
using Mneme.Testing.UsersTests;

namespace Mneme.Dashboard
{
	public class StatisticsProvider
	{
		private readonly IBundledIntegrationFacades integration;
		private readonly TestPreviewProvider testPreviewProvider;

		public StatisticsProvider(IBundledIntegrationFacades integration, TestPreviewProvider testPreviewProvider)
		{
			this.integration = integration;
			this.testPreviewProvider = testPreviewProvider;
		}

		public async Task<int> GetKnownSourcesCount(CancellationToken ct = default) => (await integration.GetKnownSources().ConfigureAwait(false)).Count;
		public async Task<int> GetKnownNotesCount(CancellationToken ct = default) => (await integration.GetKnownNotes().ConfigureAwait(false)).Count;
		public async Task<string?> GetMostRecentSource(CancellationToken ct = default) => (await integration.GetKnownSources().ConfigureAwait(false)).OrderBy(x => x.CreationTime).Select(x => x.Title).FirstOrDefault();
		public async Task<string?> GetMostRecentNote(CancellationToken ct = default) => (await integration.GetKnownNotes().ConfigureAwait(false)).OrderBy(x => x.CreationTime).Select(x => x.Title + Environment.NewLine + x.Content).FirstOrDefault();
		public async Task<int> GetAllTestsCount(CancellationToken ct = default) => (await Task.Run(() => testPreviewProvider.GetAllTests())).Count;
		public async Task<int> GetAllTestsForTestingCount(CancellationToken ct = default) => (await Task.Run(() => testPreviewProvider.GetTestsForToday())).Count;
	}
}
