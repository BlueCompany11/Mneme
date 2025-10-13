using Microsoft.EntityFrameworkCore;
using Mneme.Core;
using Mneme.DataAccess;
using Mneme.Model;

namespace Mneme.Integrations.Contracts;

public abstract class IntegrationFacadeBase<T, S, N> :
					IDatabase,
					IIntegrationFacade<S, N>
					where T : Context
					where S : Source
					where N : Note
{

	protected bool disposedValue;

	protected abstract T CreateContext();

	public virtual Task DeleteNote(int id, CancellationToken ct)
	{
		using var context = CreateContext();
		var entity = context.Set<N>().First(x => x.Id == id);
		_ = context.Set<N>().Remove(entity);
		_ = context.SaveChanges();
		return Task.CompletedTask;
	}

	public virtual Task DeleteSource(int id, CancellationToken ct)
	{
		using var context = CreateContext();
		var entity = context.Set<S>().First(x => x.Id == id);
		_ = context.Set<S>().Remove(entity);
		_ = context.SaveChanges();
		return Task.CompletedTask;
	}

	public virtual async Task<IReadOnlyList<N>> GetActiveNotes(CancellationToken ct)
	{
		using var context = CreateContext();
		return await context.Set<N>().ToListAsync(ct).ConfigureAwait(false);
	}

	public virtual async Task<IReadOnlyList<S>> GetActiveSources(CancellationToken ct)
	{
		using var context = CreateContext();
		return await context.Set<S>().Where(x => x.Active).ToListAsync(ct).ConfigureAwait(false);
	}

	public virtual Task<N> GetNote(int id, CancellationToken ct)
	{
		using var context = CreateContext();
		return Task.FromResult(context.Set<N>().First(x => x.Id == id));
	}

	public virtual async Task<IReadOnlyList<N>> GetNotes(CancellationToken ct)
	{
		using var context = CreateContext();
		return await context.Set<N>().ToListAsync(ct).ConfigureAwait(false);
	}

	public virtual async Task<IReadOnlyList<N>> GetKnownNotes(bool activeOnly, CancellationToken ct)
	{
		using var context = CreateContext();
		return await context.Set<N>().ToListAsync(ct).ConfigureAwait(false);
		//TODO activeOnly
	}

	public virtual Task<S> GetSource(int id, CancellationToken ct)
	{
		using var context = CreateContext();
		return Task.FromResult(context.Set<S>().First(x => x.Id == id));
	}

	public virtual async Task<IReadOnlyList<S>> GetSources(CancellationToken ct)
	{
		using var context = CreateContext();
		return await context.Set<S>().ToListAsync(ct).ConfigureAwait(false);
	}

	public virtual async Task<IReadOnlyList<S>> GetKnownSources(bool onlyActive, CancellationToken ct)
	{
		using var context = CreateContext();
		return await context.Set<S>().Where(x => x.Active).ToListAsync(ct).ConfigureAwait(false);
	}

	public virtual Task CreateSource(S source)
	{
		using var context = CreateContext();
		_ = context.Set<S>().Update(source);
		_ = context.SaveChanges();
		return Task.CompletedTask;
	}

	public virtual Task CreateNote(N note)
	{
		using var context = CreateContext();
		_ = context.Set<N>().Update(note);
		_ = context.SaveChanges();
		return Task.CompletedTask;
	}

	public virtual Task UpdateSource(S source, CancellationToken ct)
	{
		using var context = CreateContext();
		_ = context.Set<S>().Update(source);
		_ = context.SaveChanges();
		return Task.CompletedTask;
	}

	public async Task MigrateDatabase(CancellationToken ct = default)
	{
		using var context = CreateContext();
		await context.Database.MigrateAsync(ct).ConfigureAwait(false);
	}
}