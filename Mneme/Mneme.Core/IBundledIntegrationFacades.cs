﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mneme.Model.Notes;
using Mneme.Model.Sources;

namespace Mneme.Core
{
	public interface IBundledIntegrationFacades : IDisposable
	{
		Task ActivateSource(string id, string type, CancellationToken ct = default);
		Task IgnoreSource(string id, string type, CancellationToken ct = default);
		/// <summary>
		/// Returns sources that are active and also will check for any new sources
		/// </summary>
		/// <param name="ct"></param>
		/// <returns></returns>
		Task<IReadOnlyList<Source>> GetActiveSources(CancellationToken ct = default);
		/// <summary>
		/// Returns notes that are active and also will check for any new notes
		/// </summary>
		/// <param name="ct"></param>
		/// <returns></returns>
		Task<IReadOnlyList<Note>> GetActiveNotes(CancellationToken ct = default);
		/// <summary>
		/// Returns sources that are known to the system (checking only database)
		/// </summary>
		/// <param name="ct"></param>
		/// <returns></returns>
		Task<IReadOnlyList<Source>> GetKnownSources(bool onlyActive = true, CancellationToken ct = default);
		/// <summary>
		/// Returns notes that are known to the system (checking only database)
		/// </summary>
		/// <param name="ct"></param>
		/// <returns></returns>
		Task<IReadOnlyList<Note>> GetKnownNotes(bool onlyActive = true, CancellationToken ct = default);
		/// <summary>
		/// Returns notes that are known to the system (checking only database)
		/// </summary>
		/// <param name="ct"></param>
		/// <returns></returns>
		Task<IReadOnlyList<Note>> GetNotes(CancellationToken ct = default);
		/// <summary>
		/// Returns sources that are known to the system (checking only database)
		/// </summary>
		/// <param name="ct"></param>
		/// <returns></returns>
		Task<IReadOnlyList<Source>> GetSources(CancellationToken ct = default);
		Task<Source> GetSource(string id, string type, CancellationToken ct = default);
	}
}