using Mneme.Model;

namespace Mneme.Integrations.Contracts
{
	public interface IIntegrationFacade<S, N> : IDatabase, IDisposable
		where S : Source
		where N : Note
	{
		Task DeleteSource(int id, CancellationToken ct);
		Task UpdateSource(S source, CancellationToken ct);
		/// <summary>
		/// Only Mneme note can be deleted.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task DeleteNote(int id, CancellationToken ct);
		Task CreateSource(S source);
		Task CreateNote(N note);
		Task<IReadOnlyList<S>> GetActiveSources(CancellationToken ct);
		Task<IReadOnlyList<S>> GetSources(CancellationToken ct);
		Task<IReadOnlyList<S>> GetKnownSources(bool activeOnly, CancellationToken ct);
		Task<IReadOnlyList<N>> GetActiveNotes(CancellationToken ct);
		Task<IReadOnlyList<N>> GetNotes(CancellationToken ct);
		Task<IReadOnlyList<N>> GetKnownNotes(bool activeOnly, CancellationToken ct);
		Task<S> GetSource(int id, CancellationToken ct);
		Task<N> GetNote(int id, CancellationToken ct);
	}
}
