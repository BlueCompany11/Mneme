using Mneme.Core;
using Mneme.Model;

namespace Mneme.Sources;

public class SourcesFacade : ISourcesFacade
{
	private readonly IBundledIntegrationFacades integration;

	public SourcesFacade(IBundledIntegrationFacades integration) => this.integration = integration;

	public async Task<Source> IgnoreSource(Source source)
	{
		await integration.IgnoreSource(source.Id, source.TextType).ConfigureAwait(false);
		return await integration.GetSource(source.Id, source.TextType).ConfigureAwait(false);
	}

	public async Task<Source> ActivateSource(Source source)
	{
		await integration.ActivateSource(source.Id, source.TextType).ConfigureAwait(false);
		return await integration.GetSource(source.Id, source.TextType).ConfigureAwait(false);
	}

	public async Task<IReadOnlyList<Source>> GetKnownSourcesPreviewAsync(bool onlyActive = false, CancellationToken ct = default) => (await integration.GetKnownSources(onlyActive, ct).ConfigureAwait(false)).ToList();

	public async Task<IReadOnlyList<Source>> GetSourcesPreviewAsync(CancellationToken ct = default) => (await integration.GetSources(true, ct).ConfigureAwait(false)).ToList();
}
