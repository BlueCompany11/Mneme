using Microsoft.EntityFrameworkCore;
using Mneme.Integrations.GoogleBooks.Authorization;
using Mneme.Integrations.GoogleBooks.Database;

namespace Mneme.Integrations.GoogleBooks.Contract;

public class GoogleBooksNoteProvider
{
	private readonly GoogleBooksService googleBooksService;

	public GoogleBooksNoteProvider(GoogleBooksService googleBooksService) => this.googleBooksService = googleBooksService;

	public async Task<List<GoogleBooksNote>> GetNotesAsync(CancellationToken ct)
	{
		var ret = new List<GoogleBooksNote>();
		try
		{
			googleBooksService.Connect();
		}
		catch (FileNotFoundException)
		{
			return ret;
		}
		List<GoogleBooksNote> notes = await googleBooksService.LoadNotes(ct).ConfigureAwait(false);
		if (ct.IsCancellationRequested)
			return ret;
		//add sources
		//get unique sources
		var uniqueSources = new Dictionary<string, GoogleBooksSource>();
		foreach (GoogleBooksNote note in notes)
			_ = uniqueSources.TryAdd(note.Source.IntegrationId, note.Source);

		using var googleBooksContext = new GoogleBooksContext();
		//check if new sources are in database
		List<GoogleBooksSource> existingSources = await googleBooksContext.GoogleBooksSources.ToListAsync(ct).ConfigureAwait(false);
		foreach (GoogleBooksSource? existingSource in existingSources)
		{
			if (uniqueSources.ContainsKey(existingSource.IntegrationId))
				_ = uniqueSources.Remove(existingSource.IntegrationId);
		}

		googleBooksContext.AddRange(uniqueSources.Values);
		_ = await googleBooksContext.SaveChangesAsync(ct).ConfigureAwait(false);
		foreach (GoogleBooksNote entity in notes)
		{
			GoogleBooksNote? note = googleBooksContext.GoogleBooksNotes.FirstOrDefault(x => x.IntegrationId == entity.IntegrationId);
			if (note == null)
			{
				GoogleBooksSource source = googleBooksContext.GoogleBooksSources.First(x => x.IntegrationId == entity.IntegrationId);
				entity.Source = source;
				_ = googleBooksContext.Update(entity);
			}
		}
		_ = await googleBooksContext.SaveChangesAsync(ct).ConfigureAwait(false);
		foreach (GoogleBooksNote? item in googleBooksContext.GoogleBooksNotes.Where(x => x.Source.Active))
		{
			ret.Add(item);
		}
		return ret;
	}
}

