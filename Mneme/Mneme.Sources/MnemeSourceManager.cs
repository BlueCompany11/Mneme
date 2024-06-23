using Mneme.Core.Interfaces;
using Mneme.Integrations.Mneme.Contract;

namespace Mneme.Sources
{
	public class MnemeSourceManager : IDisposable
	{
		private readonly IBundledIntegrationFacades integration;

		public MnemeSourceManager(IBundledIntegrationFacades integration)
		{
			this.integration = integration;
		}

		public async Task<MnemeSource?> SaveMnemeSource(string sourceTitle, string details, CancellationToken ct)
		{
			var source = new MnemeSource { Title = sourceTitle, Details = details, Active = true, IntegrationId = MnemeSource.GenerateIntegrationId(sourceTitle, details) };
			var success = await integration.Create(source, ct);
			return success ? source : null;
		}

		public async Task <MnemeSource?> UpdateMnemeSource(string id, string title, string details, CancellationToken ct)
		{
			await integration.UpdateSource(id, title, details);
			return (MnemeSource)await integration.GetSource(id, MnemeSource.Type);
		}

		public void Dispose()
		{
			integration.Dispose();
		}
	}
}
