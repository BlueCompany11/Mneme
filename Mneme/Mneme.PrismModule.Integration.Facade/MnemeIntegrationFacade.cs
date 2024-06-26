using System;
using System.Threading;
using System.Threading.Tasks;
using Mneme.Core;
using Mneme.Integrations.Contracts;
using Mneme.Integrations.Mneme.Contract;

namespace Mneme.PrismModule.Integration.Facade
{
	public class MnemeIntegrationFacade : IMnemeIntegrationFacade
	{
		private readonly IIntegrationFacade<MnemeSource, MnemeNote> mnemeIntegration;

		public MnemeIntegrationFacade(IIntegrationFacade<MnemeSource, MnemeNote> mnemeIntegration)
		{
			this.mnemeIntegration = mnemeIntegration;
		}
		public async Task DeleteNote(string id, CancellationToken ct = default)
		{
			await mnemeIntegration.DeleteNote(id, ct);
		}

		public async Task DeleteSource(string id, CancellationToken ct = default)
		{
			await mnemeIntegration.DeleteSource(id, ct);
		}

		public async Task UpdateSource(string id, string title, string details, CancellationToken ct = default)
		{
			var source = await mnemeIntegration.GetSource(id, ct);
			source.Title = title;
			source.Details = details;
			await mnemeIntegration.UpdateSource(source, ct);
		}

		public async Task<bool> Create(MnemeSource source, CancellationToken ct = default)
		{
			//work around to problem where integration id's were different but for some reason db was telling it already exists
			try
			{
				var sourceDb = await mnemeIntegration.GetSource(source.IntegrationId, ct);
				return false;
			}
			catch (InvalidOperationException)
			{
				await mnemeIntegration.CreateSource(source);
				return true;
			}
		}

		public async Task CreateNote(MnemeNote note, CancellationToken ct = default)
		{
			await mnemeIntegration.CreateNote(note);
		}
	}
}
