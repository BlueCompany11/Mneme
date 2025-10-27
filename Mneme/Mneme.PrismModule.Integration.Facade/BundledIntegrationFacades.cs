using Mneme.Core;
using Mneme.Integrations.Contracts;
using Mneme.Integrations.GoogleBooks.Contract;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mneme.PrismModule.Integration.Facade;

public class BundledIntegrationFacades : IBundledIntegrationFacades
{
	private readonly IIntegrationFacade<GoogleBooksSource, GoogleBooksNote> googleBooksIntegration;
	private readonly IIntegrationFacade<MnemeSource, MnemeNote> mnemeIntegration;

	public BundledIntegrationFacades(
			IIntegrationFacade<GoogleBooksSource, GoogleBooksNote> googleBooksIntegration,
			IIntegrationFacade<MnemeSource, MnemeNote> mnemeIntegration
			)
	{
		this.googleBooksIntegration = googleBooksIntegration;
		this.mnemeIntegration = mnemeIntegration;
	}

	public async Task ActivateSource(int id, string type, CancellationToken ct = default)
	{
		if (type == GoogleBooksSource.Type)
		{
			var source = await googleBooksIntegration.GetSource(id, ct).ConfigureAwait(false);
			source.Active = true;
			await googleBooksIntegration.UpdateSource(source, ct).ConfigureAwait(false);
		} else if (type == MnemeSource.Type)
		{
			var source = await mnemeIntegration.GetSource(id, ct).ConfigureAwait(false);
			source.Active = true;
			await mnemeIntegration.UpdateSource(source, ct).ConfigureAwait(false);
		}
	}

	public async Task<IReadOnlyList<INote>> GetNotes(bool activeOnly, CancellationToken ct = default)
	{
		var googleBooksTask = googleBooksIntegration.GetNotes(ct);
		var mnemeTask = mnemeIntegration.GetNotes(ct);

		await Task.WhenAll(googleBooksTask, mnemeTask).ConfigureAwait(false);

		var ret = new List<INote>();
		ret.AddRange(await googleBooksTask);
		ret.AddRange(await mnemeTask);
		return ret;
	}

	public async Task<IReadOnlyList<ISource>> GetSources(bool activeOnly, CancellationToken ct = default)
	{
		var googleBooksTask = googleBooksIntegration.GetSources(ct);
		var mnemeTask = mnemeIntegration.GetSources(ct);

		await Task.WhenAll(googleBooksTask, mnemeTask).ConfigureAwait(false);

		var ret = new List<ISource>();
		ret.AddRange(await googleBooksTask);
		ret.AddRange(await mnemeTask);
		return ret;
	}
	public async Task<IReadOnlyList<ISource>> GetKnownSources(bool onlyActive = true, CancellationToken ct = default)
	{
		var googleBooksTask = googleBooksIntegration.GetKnownSources(onlyActive, ct);
		var mnemeTask = mnemeIntegration.GetKnownSources(onlyActive, ct);

		await Task.WhenAll(googleBooksTask, mnemeTask).ConfigureAwait(false);

		var ret = new List<ISource>();
		ret.AddRange(await googleBooksTask);
		ret.AddRange(await mnemeTask);
		return ret;
	}

	public async Task<IReadOnlyList<INote>> GetKnownNotes(bool onlyActive = true, CancellationToken ct = default)
	{
		var googleBooksTask = googleBooksIntegration.GetKnownNotes(onlyActive, ct);
		var mnemeTask = mnemeIntegration.GetKnownNotes(onlyActive, ct);

		await Task.WhenAll(googleBooksTask, mnemeTask).ConfigureAwait(false);

		var ret = new List<INote>();
		ret.AddRange(await googleBooksTask);
		ret.AddRange(await mnemeTask);
		return ret;
	}

	public async Task IgnoreSource(int id, string type, CancellationToken ct = default)
	{
		if (type == GoogleBooksSource.Type)
		{
			var source = await googleBooksIntegration.GetSource(id, ct).ConfigureAwait(false);
			source.Active = false;
			await googleBooksIntegration.UpdateSource(source, ct).ConfigureAwait(false);
		} else if (type == MnemeSource.Type)
		{
			var source = await mnemeIntegration.GetSource(id, ct).ConfigureAwait(false);
			source.Active = false;
			await mnemeIntegration.UpdateSource(source, ct).ConfigureAwait(false);
		}
	}

	public async Task<ISource> GetSource(int id, string type, CancellationToken ct)
	{
		if (type == GoogleBooksSource.Type)
			return await googleBooksIntegration.GetSource(id, ct).ConfigureAwait(false);
		else if (type == MnemeSource.Type)
		{
			return await mnemeIntegration.GetSource(id, ct).ConfigureAwait(false);
		}
		throw new ArgumentException("Type value didn't match to any of the source types");
	}
}
