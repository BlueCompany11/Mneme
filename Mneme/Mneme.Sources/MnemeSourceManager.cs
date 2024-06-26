using Mneme.Core;
using Mneme.Integrations.Mneme.Contract;

namespace Mneme.Sources
{
	public class MnemeSourceManager : IDisposable
	{
		private readonly IBundledIntegrationFacades integration;
		private readonly IMnemeIntegrationFacade mnemeIntegration;

		public MnemeSourceManager(IBundledIntegrationFacades integration, IMnemeIntegrationFacade mnemeIntegration)
		{
			this.integration = integration;
			this.mnemeIntegration = mnemeIntegration;
		}

		public async Task<MnemeSource?> SaveMnemeSource(string sourceTitle, string details, CancellationToken ct)
		{
			var source = new MnemeSource { Title = sourceTitle, Details = details, Active = true, IntegrationId = MnemeSource.GenerateIntegrationId(sourceTitle, details) };
			var success = await mnemeIntegration.Create(source, ct);
			return success ? source : null;
		}

		public async Task <MnemeSource?> UpdateMnemeSource(string id, string title, string details, CancellationToken ct)
		{
			await mnemeIntegration.UpdateSource(id, title, details);
			return (MnemeSource)await integration.GetSource(id, MnemeSource.Type);
		}

		public void Dispose()
		{
			integration.Dispose();
		}
	}
}
