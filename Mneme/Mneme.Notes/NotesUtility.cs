using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Books.v1.Data;
using Microsoft.VisualBasic;
using Mneme.Core;
using Mneme.Model.Notes;

namespace Mneme.Notes
{
	public class NotesUtility
	{
		private readonly IBundledIntegrationFacades integration;
		private readonly IMnemeIntegrationFacade mnemeIntegration;

		public NotesUtility(IBundledIntegrationFacades integration, IMnemeIntegrationFacade mnemeIntegration)
		{
			this.integration = integration;
			this.mnemeIntegration = mnemeIntegration;
		}
		public async Task<IReadOnlyList<Note>> GetNotes(CancellationToken ct)
		{
			return await integration.GetNotes(ct);
		}
		public async Task DeleteNote(NotePreview preview)
		{
			await mnemeIntegration.DeleteNote(preview.Preelaboration.IntegrationId);
		}
	}
}
