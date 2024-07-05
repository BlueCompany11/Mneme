using Mneme.Core;
using Mneme.Integrations.Contracts;
using Mneme.Integrations.Mneme.Contract;

namespace Mneme.Sources
{
	public class MnemeSourceManager : IDisposable
	{
		private readonly IBundledIntegrationFacades integration;
		private readonly IIntegrationFacade<MnemeSource, MnemeNote> mnemeIntegration;

		public MnemeSourceManager(IBundledIntegrationFacades integration, IIntegrationFacade<MnemeSource, MnemeNote> mnemeIntegration)
		{
			this.integration = integration;
			this.mnemeIntegration = mnemeIntegration;
		}

		public async Task<MnemeSource?> SaveMnemeSource(string sourceTitle, string details, CancellationToken ct)
		{
			var source = new MnemeSource { Title = sourceTitle, Details = details, Active = true, IntegrationId = MnemeSource.GenerateIntegrationId(sourceTitle, details) };
			try
			{
				await mnemeIntegration.CreateSource(source);
				return await mnemeIntegration.GetSource(source.Id, ct);
			}
			catch (Exception)
			{
				return null;
			}
		}

		public async Task<MnemeSource?> UpdateMnemeSource(int id, string title, string details, CancellationToken ct)
		{
			var existingSource = await mnemeIntegration.GetSource(id, ct);
			existingSource.Title = title;
			existingSource.Details = details;
			existingSource.IntegrationId = MnemeSource.GenerateIntegrationId(title, details); //important to remember to update the IntegrationId
			await mnemeIntegration.UpdateSource(existingSource, ct);
			return (MnemeSource)await integration.GetSource(id, MnemeSource.Type);
		}

		public void Dispose()
		{
			integration.Dispose();
		}
	}
}
