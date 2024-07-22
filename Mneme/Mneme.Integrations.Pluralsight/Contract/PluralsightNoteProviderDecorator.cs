using Microsoft.EntityFrameworkCore;
using Mneme.Integrations.Pluralsight.Database;
using Mneme.Model;

namespace Mneme.Integrations.Pluralsight.Contract;

public class PluralsightNoteProviderDecorator
{
	private readonly PluralsightNoteProvider pluralsightNoteProvider;
	private readonly PluralsightConfigProvider pluralsightConfigProvider;

	public PluralsightNoteProviderDecorator(PluralsightConfigProvider pluralsightConfigProvider)
	{
		pluralsightNoteProvider = new();
		this.pluralsightConfigProvider = pluralsightConfigProvider;
	}

	public async Task<List<Note>> GetNotesAsync(CancellationToken ct = default)
	{
		var ret = new List<Note>();
		using var pluralsightContext = new PluralsightContext();
		if (pluralsightNoteProvider.TryOpen(pluralsightConfigProvider.Config.FilePath, out List<PluralsightNote>? note))
		{
			await AddNewSources(note, pluralsightContext, ct).ConfigureAwait(false);
			_ = await pluralsightContext.SaveChangesAsync(ct).ConfigureAwait(false);
			AddNewNotes(note, pluralsightContext);
			_ = await pluralsightContext.SaveChangesAsync(ct).ConfigureAwait(false);
		}
		foreach (PluralsightNote? item in pluralsightContext.PluralsightNotes.Where(x => x.Source.Active))
		{
			ret.Add(item);
		}
		return ret;
	}

	private async Task AddNewSources(List<PluralsightNote> notes, PluralsightContext pluralsightContext, CancellationToken ct)
	{
		var uniqueSources = notes.GroupBy(x => x.Source.IntegrationId).Select(x => x.First().Source).ToList();
		List<PluralsightSource> existingSources = await pluralsightContext.PluralsightSources.ToListAsync(ct).ConfigureAwait(false);
		foreach (PluralsightSource? source in uniqueSources)
		{
			PluralsightSource? existingSource = existingSources.FirstOrDefault(x => x.IntegrationId == source.IntegrationId);
			if (existingSource == null)
				_ = pluralsightContext.Update(source);
		}
	}

	private void AddNewNotes(List<PluralsightNote> notes, PluralsightContext pluralsightContext)
	{
		foreach (PluralsightNote note in notes)
		{
			PluralsightNote? existingNote = pluralsightContext.PluralsightNotes.FirstOrDefault(x => x.IntegrationId == note.IntegrationId);
			if (existingNote == null)
			{
				PluralsightSource source = pluralsightContext.PluralsightSources.First(x => x.IntegrationId == note.Source.IntegrationId);
				note.Source = source;
				_ = pluralsightContext.Update(note);
			}
		}
	}
}
