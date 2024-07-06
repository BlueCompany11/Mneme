using Microsoft.EntityFrameworkCore;
using Mneme.DataAccess;
using Mneme.Model;

namespace Mneme.Integrations.Contracts
{
	public abstract class IntegrationFacadeBase<T, S, N> :
					IDatabase,
					IIntegrationFacade<S, N>,
					IDisposable
					where T : Context
					where S : Source
					where N : Note
	{
		protected readonly T context;
		protected bool disposedValue;

		public IntegrationFacadeBase()
		{
			context = CreateContext();
		}

		protected abstract T CreateContext();

		public virtual Task DeleteNote(int id, CancellationToken ct)
		{
			var entity = context.Set<N>().First(x => x.Id == id);
			context.Set<N>().Remove(entity);
			context.SaveChanges();
			return Task.CompletedTask;
		}

		public virtual Task DeleteSource(int id, CancellationToken ct)
		{
			var entity = context.Set<S>().First(x => x.Id == id);
			context.Set<S>().Remove(entity);
			context.SaveChanges();
			return Task.CompletedTask;
		}

		public virtual async Task<IReadOnlyList<N>> GetActiveNotes(CancellationToken ct)
		{
			return await context.Set<N>().ToListAsync(ct);
		}

		public virtual async Task<IReadOnlyList<S>> GetActiveSources(CancellationToken ct)
		{
			return await context.Set<S>().Where(x => x.Active).ToListAsync(ct);
		}

		public virtual Task<N> GetNote(int id, CancellationToken ct)
		{
			return Task.FromResult(context.Set<N>().First(x => x.Id == id));
		}

		public virtual async Task<IReadOnlyList<N>> GetNotes(CancellationToken ct)
		{
			return await context.Set<N>().ToListAsync(ct);
		}

		public virtual async Task<IReadOnlyList<N>> GetKnownNotes(bool activeOnly, CancellationToken ct)
		{
			return await context.Set<N>().ToListAsync(ct);
			//TODO activeOnly
		}

		public virtual Task<S> GetSource(int id, CancellationToken ct)
		{
			return Task.FromResult(context.Set<S>().First(x => x.Id == id));
		}

		public virtual async Task<IReadOnlyList<S>> GetSources(CancellationToken ct)
		{
			return await context.Set<S>().ToListAsync(ct);
		}

		public virtual async Task<IReadOnlyList<S>> GetKnownSources(bool onlyActive, CancellationToken ct)
		{
			return await context.Set<S>().Where(x => x.Active).ToListAsync(ct);
		}

		public virtual Task CreateSource(S source)
		{
			context.Set<S>().Update(source);
			context.SaveChanges();
			return Task.CompletedTask;
		}

		public virtual Task CreateNote(N note)
		{
			context.Set<N>().Update(note);
			context.SaveChanges();
			return Task.CompletedTask;
		}

		public virtual Task UpdateSource(S source, CancellationToken ct)
		{
			context.Set<S>().Update(source);
			context.SaveChanges();
			return Task.CompletedTask;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					context.Dispose();
				}

				disposedValue = true;
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		public async Task MigrateDatabase(CancellationToken ct = default)
		{
			using var context = CreateContext();
			await context.Database.MigrateAsync(ct);
		}
	}
}