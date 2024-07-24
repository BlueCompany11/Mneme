using Mneme.Core;
using Mneme.Integrations.Contracts;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Model;

namespace Mneme.Notes;

public class NotesUtility : INotesUtility
{
	private readonly IBundledIntegrationFacades integration;

	public NotesUtility(IBundledIntegrationFacades integration)
	{
		this.integration = integration;
	}
	public async Task<IReadOnlyList<Note>> GetNotes(CancellationToken ct) => await integration.GetNotes(false, ct).ConfigureAwait(false);

}
