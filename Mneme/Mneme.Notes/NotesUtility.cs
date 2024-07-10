using Mneme.Core;
using Mneme.Integrations.Contracts;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Model;

namespace Mneme.Notes
{
	public class NotesUtility
	{
		private readonly IBundledIntegrationFacades integration;
		private readonly IIntegrationFacade<MnemeSource, MnemeNote> mnemeIntegration;

		public NotesUtility(IBundledIntegrationFacades integration, IIntegrationFacade<MnemeSource, MnemeNote> mnemeIntegration)
		{
			this.integration = integration;
			this.mnemeIntegration = mnemeIntegration;
		}
		public async Task<IReadOnlyList<Note>> GetNotes(CancellationToken ct)
		{
			return await integration.GetNotes(false, ct).ConfigureAwait(false);
		}
		public async Task DeleteNote(Note note)
		{
			await mnemeIntegration.DeleteNote(note.Id, default).ConfigureAwait(false);
		}
	}
}
