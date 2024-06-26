using Microsoft.EntityFrameworkCore;
using Mneme.Core;
using Mneme.Model.Sources;

namespace Mneme.Sources
{
	public class SourcesManager : IDisposable
	{
		private readonly IBundledIntegrationFacades integration;
		private readonly IMnemeIntegrationFacade mnemeIntegration;

		public SourcesManager(IBundledIntegrationFacades integration, IMnemeIntegrationFacade mnemeIntegration)
		{
			this.integration = integration;
			this.mnemeIntegration = mnemeIntegration;
		}
		public async Task<bool> DeleteSource(SourcePreview source)
		{
			try
			{
				await mnemeIntegration.DeleteSource(source.Id);
				return true;
			}
			catch (DbUpdateException)
			{
				return false;
			}
		}
		public async Task<SourcePreview> IgnoreSource(SourcePreview source)
		{
			await integration.IgnoreSource(source.Id, source.TypeOfSource);
			var updatedSource = await integration.GetSource(source.Id, source.TypeOfSource);
			return SourcePreview.CreateFromSource(updatedSource);
		}

		public async Task<SourcePreview> ActivateSource(SourcePreview source)
		{
			await integration.ActivateSource(source.Id, source.TypeOfSource);
			var updatedSource = await integration.GetSource(source.Id, source.TypeOfSource);
			return SourcePreview.CreateFromSource(updatedSource);
		}

		public async Task<IReadOnlyList<SourcePreview>> GetKnownSourcesPreviewAsync(bool onlyActive = false, CancellationToken ct = default)
		{
			return (await integration.GetKnownSources(onlyActive, ct)).Select(x => SourcePreview.CreateFromSource(x)).ToList();
		}

		public async Task<IReadOnlyList<SourcePreview>> GetSourcesPreviewAsync(CancellationToken ct = default)
		{
			return (await integration.GetSources(ct)).Select(x => SourcePreview.CreateFromSource(x)).ToList();
		}

		public void Dispose()
		{
			integration.Dispose();
		}
	}
}
