using Mneme.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mneme.Core;

public interface IBundledIntegrationFacades
{
	Task ActivateSource(int id, string type, CancellationToken ct = default);
	Task IgnoreSource(int id, string type, CancellationToken ct = default);
	Task<IReadOnlyList<ISource>> GetKnownSources(bool onlyActive = true, CancellationToken ct = default);
	/// <summary>
	/// Returns notes that are known to the system (checking only database)
	/// </summary>
	/// <param name="ct"></param>
	/// <returns></returns>
	Task<IReadOnlyList<Note>> GetKnownNotes(bool onlyActive = true, CancellationToken ct = default);
	/// <summary>
	/// Returns sources that are active and also will check for any new sources
	/// </summary>
	/// <param name="ct"></param>
	/// <returns></returns>
	Task<IReadOnlyList<Note>> GetNotes(bool activeOnly, CancellationToken ct = default);
	/// <summary>
	/// Returns sources that are active and also will check for any new sources
	/// </summary>
	/// <param name="ct"></param>
	/// <returns></returns>
	Task<IReadOnlyList<ISource>> GetSources(bool activeOnly, CancellationToken ct = default);
	Task<ISource> GetSource(int id, string type, CancellationToken ct = default);
}