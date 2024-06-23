using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Books.v1.Data;
using Microsoft.VisualBasic;
using Mneme.Core.Interfaces;
using Mneme.Model.Preelaborations;

namespace Mneme.Notes
{
	public class NotesUtility
	{
		private readonly IBundledIntegrationFacades integration;

		public NotesUtility(IBundledIntegrationFacades integration)
		{
			this.integration = integration;
		}
		public async Task<IReadOnlyList<Preelaboration>> GetNotes(CancellationToken ct)
		{
			return await integration.GetNotes(ct);
		}
		public async Task DeleteNote(PreelaborationPreview preview)
		{
			await integration.DeleteNote(preview.Preelaboration.IntegrationId);
		}
	}
}
