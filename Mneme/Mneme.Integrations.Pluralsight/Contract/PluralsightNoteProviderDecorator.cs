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

	public async Task<List<INote>> GetNotesAsync(CancellationToken ct = default)
	{
		var ret = new List<INote>();
		using var pluralsightContext = new PluralsightContext();
		if (pluralsightNoteProvider.TryOpen(pluralsightConfigProvider.Config.FilePath, out var note))
		{
			await AddNewSources(note, pluralsightContext, ct).ConfigureAwait(false);
			_ = await pluralsightContext.SaveChangesAsync(ct).ConfigureAwait(false);
			AddNewNotes(note, pluralsightContext);
			_ = await pluralsightContext.SaveChangesAsync(ct).ConfigureAwait(false);
		}
		foreach (var item in pluralsightContext.PluralsightNotes.Where(x => x.Source.Active))
		{
			ret.Add(item);
		}
		return ret;
	}

	private async Task AddNewSources(List<PluralsightNote> notes, PluralsightContext pluralsightContext, CancellationToken ct)
	{
		var uniqueSources = notes.GroupBy(x => x.Source.IntegrationId).Select(x => x.First().Source).ToList();
		var existingSources = await pluralsightContext.PluralsightSources.ToListAsync(ct).ConfigureAwait(false);
		foreach (var source in uniqueSources)
		{
			var existingSource = existingSources.FirstOrDefault(x => x.IntegrationId == source.IntegrationId);
			if (existingSource == null)
				_ = pluralsightContext.Update(source);
		}
	}

	private void AddNewNotes(List<PluralsightNote> notes, PluralsightContext pluralsightContext)
	{
		foreach (var note in notes)
		{
			var existingNote = pluralsightContext.PluralsightNotes.FirstOrDefault(x => x.IntegrationId == note.IntegrationId);
			if (existingNote == null)
			{
				var source = pluralsightContext.PluralsightSources.First(x => x.IntegrationId == note.Source.IntegrationId);
				note.Source = source;
				_ = pluralsightContext.Update(note);
			}
		}
	}
}
