using Microsoft.EntityFrameworkCore;
using Mneme.Core;
using Mneme.Integrations.Contracts;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Model;

namespace Mneme.Sources;

public class SourcesManager
{
	private readonly IBundledIntegrationFacades integration;
	private readonly IIntegrationFacade<MnemeSource, MnemeNote> mnemeIntegration;

	public SourcesManager(IBundledIntegrationFacades integration, IIntegrationFacade<MnemeSource, MnemeNote> mnemeIntegration)
	{
		this.integration = integration;
		this.mnemeIntegration = mnemeIntegration;
	}
	public async Task<bool> DeleteSource(Source source)
	{
		try
		{
			await mnemeIntegration.DeleteSource(source.Id, default).ConfigureAwait(false);
			return true;
		}
		catch (DbUpdateException)
		{
			return false;
		}
	}
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
