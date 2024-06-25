using Microsoft.EntityFrameworkCore;
using Mneme.Integrations.Pluralsight.Database;
using Mneme.Model.Preelaborations;

namespace Mneme.Integrations.Pluralsight.Contract
{
	public class PluralsightPreelaborationProviderDecorator
	{
		private readonly PluralsightPreelaborationProvider pluralsightPreelaborationProvider;
		private readonly PluralsightConfigProvider pluralsightConfigProvider;

		public PluralsightPreelaborationProviderDecorator(PluralsightConfigProvider pluralsightConfigProvider)
		{
			this.pluralsightPreelaborationProvider = new();
			this.pluralsightConfigProvider = pluralsightConfigProvider;
		}

		public async Task<List<Preelaboration>> GetPreelaborationsAsync(CancellationToken ct = default)
		{
			var ret = new List<Preelaboration>();
			using var pluralsightContext = new PluralsightContext();
			if (pluralsightPreelaborationProvider.TryOpen(pluralsightConfigProvider.Config.FilePath, out var pre))
			{
				await AddNewSources(pre, pluralsightContext, ct);
				_ = await pluralsightContext.SaveChangesAsync(ct);
				AddNewNotes(pre, pluralsightContext);
				_ = await pluralsightContext.SaveChangesAsync(ct);
			}
			foreach (var item in pluralsightContext.PluralsightPreelaboration.Where(x => x.Source.Active))
			{
				ret.Add(item);
			}
			return ret;
		}

		private async Task AddNewSources(List<PluralsightPreelaboration> pre, PluralsightContext pluralsightContext, CancellationToken ct)
		{
			var uniqueSources = pre.GroupBy(x => x.Source.IntegrationId).Select(x => x.First().Source).ToList();
			var existingSources = await pluralsightContext.PluralsightSources.ToListAsync(ct);
			foreach (var source in uniqueSources)
			{
				var existingSource = existingSources.FirstOrDefault(x => x.IntegrationId == source.IntegrationId);
				if (existingSource == null)
					_ = pluralsightContext.Update(source);
			}
		}

		private void AddNewNotes(List<PluralsightPreelaboration> notes, PluralsightContext pluralsightContext)
		{
			foreach (var note in notes)
			{
				var existingNote = pluralsightContext.PluralsightPreelaboration.FirstOrDefault(x => x.IntegrationId == note.IntegrationId);
				if (existingNote == null)
				{
					var source = pluralsightContext.PluralsightSources.First(x => x.IntegrationId == note.Source.IntegrationId);
					note.Source = source;
					_ = pluralsightContext.Update(note);
				}
			}
		}
	}
}
