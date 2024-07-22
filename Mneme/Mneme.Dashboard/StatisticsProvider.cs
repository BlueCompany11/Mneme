using Mneme.Core;


namespace Mneme.Dashboard;

public class StatisticsProvider : IStatisticsProvider
{
	private readonly IBundledIntegrationFacades integration;
	private readonly ITestProvider testProvider;

	public StatisticsProvider(IBundledIntegrationFacades integration, ITestProvider testProvider)
	{
		this.integration = integration;
		this.testProvider = testProvider;
	}

	public async Task<int> GetKnownSourcesCount(CancellationToken ct = default) => (await integration.GetKnownSources(true, ct).ConfigureAwait(false)).Count;
	public async Task<int> GetKnownNotesCount(CancellationToken ct = default) => (await integration.GetKnownNotes(true, ct).ConfigureAwait(false)).Count;
	public async Task<string?> GetMostRecentSource(CancellationToken ct = default) => (await integration.GetKnownSources(true, ct).ConfigureAwait(false)).OrderBy(x => x.CreationTime).Select(x => x.Title).FirstOrDefault();
	public async Task<string?> GetMostRecentNote(CancellationToken ct = default) => (await integration.GetKnownNotes(true, ct).ConfigureAwait(false)).OrderBy(x => x.CreationTime).Select(x => x.Title + Environment.NewLine + x.Content).FirstOrDefault();
	public async Task<int> GetAllTestsCount(CancellationToken ct = default) => (await testProvider.GetAllTests(ct)).Count;
	public async Task<int> GetAllTestsForTestingCount(CancellationToken ct = default) => (await testProvider.GetTestsForToday(ct)).Count;
}

public interface IStatisticsProvider
{
	Task<int> GetKnownSourcesCount(CancellationToken ct = default);
	Task<int> GetKnownNotesCount(CancellationToken ct = default);
	Task<string?> GetMostRecentSource(CancellationToken ct = default);
	Task<string?> GetMostRecentNote(CancellationToken ct = default);
	Task<int> GetAllTestsCount(CancellationToken ct = default);
	Task<int> GetAllTestsForTestingCount(CancellationToken ct = default);
}
