using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Model.Preelaborations;
using Mneme.Model.Sources;

namespace Mneme.Core.Interfaces
{
	public interface IBundledIntegrationFacades : IDisposable
	{
		Task ActivateSource(string id, string type, CancellationToken ct = default);
		Task DeleteNote(string id, CancellationToken ct = default);
		Task DeleteSource(string id, CancellationToken ct = default);
		Task<IReadOnlyList<Preelaboration>> GetActiveNotes(CancellationToken ct = default);
		Task<IReadOnlyList<Source>> GetActiveSources(CancellationToken ct = default);
		Task<IReadOnlyList<Source>> GetKnownSources(bool onlyActive, CancellationToken ct = default);
		Task<IReadOnlyList<Preelaboration>> GetNotes(CancellationToken ct = default);
		Task<IReadOnlyList<Source>> GetSources(CancellationToken ct = default);
		Task IgnoreSource(string id, string type, CancellationToken ct = default);
		Task MigrateDatabase(CancellationToken ct = default);
		Task<bool> Create(MnemeSource source, CancellationToken ct = default);
		Task CreateNote(MnemePreelaboration note, CancellationToken ct = default);
		Task UpdateSource(string id, string title, string details, CancellationToken ct = default);
		Task<Source> GetSource(string id, string type, CancellationToken ct = default);
	}
}