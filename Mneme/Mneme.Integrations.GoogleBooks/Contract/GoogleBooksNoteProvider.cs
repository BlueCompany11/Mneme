﻿using Microsoft.EntityFrameworkCore;
using Mneme.Integrations.GoogleBooks.Authorization;
using Mneme.Integrations.GoogleBooks.Database;

namespace Mneme.Integrations.GoogleBooks.Contract
{
	public class GoogleBooksNoteProvider
	{
		private readonly GoogleBooksService googleBooksService;

		public GoogleBooksNoteProvider(GoogleBooksService googleBooksService)
		{
			this.googleBooksService = googleBooksService;
		}

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
			var notes = await googleBooksService.LoadNotes(ct);
			if (ct.IsCancellationRequested)
				return ret;
			//add sources
			//get unique sources
			var uniqueSources = new Dictionary<string, GoogleBooksSource>();
			foreach (var note in notes)
				uniqueSources.TryAdd(note.Source.IntegrationId, note.Source);

			using var googleBooksContext = new GoogleBooksContext();
			//check if new sources are in database
			var existingSources = await googleBooksContext.GoogleBooksSources.ToListAsync(ct);
			foreach (var existingSource in existingSources)
			{
				if (uniqueSources.ContainsKey(existingSource.IntegrationId))
					_ = uniqueSources.Remove(existingSource.IntegrationId);
			}

			googleBooksContext.AddRange(uniqueSources.Values);
			await googleBooksContext.SaveChangesAsync(ct);
			foreach (var entity in notes)
			{
				var note = googleBooksContext.GoogleBooksNotes.FirstOrDefault(x => x.IntegrationId == entity.IntegrationId);
				if (note == null)
				{
					var source = googleBooksContext.GoogleBooksSources.First(x => x.IntegrationId == entity.IntegrationId);
					entity.Source = source;
					googleBooksContext.Update(entity);
				}
			}
			_ = await googleBooksContext.SaveChangesAsync(ct);
			foreach (var item in googleBooksContext.GoogleBooksNotes.Where(x => x.Source.Active))
			{
				ret.Add(item);
			}
			return ret;
		}
	}
}

