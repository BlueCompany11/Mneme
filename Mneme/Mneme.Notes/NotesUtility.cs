using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Books.v1.Data;
using Microsoft.VisualBasic;
using Mneme.Core;
using Mneme.Integrations.Contracts;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Model.Notes;

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
			return await integration.GetNotes(false, ct);
		}
		public async Task DeleteNote(NotePreview preview)
		{
			await mnemeIntegration.DeleteNote(preview.BaseNote.IntegrationId, default);
		}
	}
}
