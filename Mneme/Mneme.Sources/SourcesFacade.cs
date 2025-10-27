using Mneme.Core;
using Mneme.Model;

namespace Mneme.Sources;

public class SourcesFacade : ISourcesFacade
{
	private readonly IBundledIntegrationFacades integration;

	public SourcesFacade(IBundledIntegrationFacades integration) => this.integration = integration;

	public async Task<ISource> IgnoreSource(ISource source)
	{
		await integration.IgnoreSource(source.Id, source.TextType).ConfigureAwait(false);
		return await integration.GetSource(source.Id, source.TextType).ConfigureAwait(false);
	}

	public async Task<ISource> ActivateSource(ISource source)
	{
		await integration.ActivateSource(source.Id, source.TextType).ConfigureAwait(false);
		return await integration.GetSource(source.Id, source.TextType).ConfigureAwait(false);
	}

	public async Task<IReadOnlyList<ISource>> GetKnownSourcesPreviewAsync(bool onlyActive = false, CancellationToken ct = default) => (await integration.GetKnownSources(onlyActive, ct).ConfigureAwait(false)).ToList();

	public async Task<IReadOnlyList<ISource>> GetSourcesPreviewAsync(CancellationToken ct = default) => (await integration.GetSources(true, ct).ConfigureAwait(false)).ToList();
}
