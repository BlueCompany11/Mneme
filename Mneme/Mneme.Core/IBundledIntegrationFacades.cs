using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Model.Notes;
using Mneme.Model.Sources;

namespace Mneme.Core
{
	public interface IBundledIntegrationFacades : IDisposable
	{
		Task ActivateSource(string id, string type, CancellationToken ct = default);
		Task<IReadOnlyList<Note>> GetActiveNotes(CancellationToken ct = default);
		Task<IReadOnlyList<Source>> GetActiveSources(CancellationToken ct = default);
		Task<IReadOnlyList<Source>> GetKnownSources(bool onlyActive, CancellationToken ct = default);
		Task<IReadOnlyList<Note>> GetNotes(CancellationToken ct = default);
		Task<IReadOnlyList<Source>> GetSources(CancellationToken ct = default);
		Task IgnoreSource(string id, string type, CancellationToken ct = default);
		Task<Source> GetSource(string id, string type, CancellationToken ct = default);
	}
}