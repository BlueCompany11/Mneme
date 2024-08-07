﻿using Mneme.Integrations.Contracts;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Model;

namespace Mneme.Notes;

public class MnemeNotesProxy : IMnemeNotesProxy
{
	private readonly IIntegrationFacade<MnemeSource, MnemeNote> mnemeIntegration;

	public MnemeNotesProxy(IIntegrationFacade<MnemeSource, MnemeNote> mnemeIntegration) => this.mnemeIntegration = mnemeIntegration;
	public async Task<MnemeNote> SaveMnemeNote(Source source, string content, string title, string path, CancellationToken ct)
	{
		MnemeSource newSource = await mnemeIntegration.GetSource(source.Id, ct);
		var note = new MnemeNote() { Content = content, Title = title, Path = path, Source = newSource };
		await mnemeIntegration.CreateNote(note).ConfigureAwait(false);
		return note;
	}

	public async Task<IReadOnlyList<Source>> GetMnemeSources(CancellationToken ct) => await mnemeIntegration.GetActiveSources(ct).ConfigureAwait(false);

	public async Task DeleteNote(Note note) => await mnemeIntegration.DeleteNote(note.Id, default).ConfigureAwait(false);
}
