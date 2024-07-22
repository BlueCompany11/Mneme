using Mneme.Core;
using Mneme.Integrations.Contracts;
using Mneme.Integrations.GoogleBooks.Contract;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Integrations.Pluralsight.Contract;
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
	private readonly IIntegrationFacade<PluralsightSource, PluralsightNote> pluralsightIntegration;

	public BundledIntegrationFacades(
			IIntegrationFacade<GoogleBooksSource, GoogleBooksNote> googleBooksIntegration,
			IIntegrationFacade<MnemeSource, MnemeNote> mnemeIntegration,
			IIntegrationFacade<PluralsightSource, PluralsightNote> pluralsightIntegration
			)
	{
		this.googleBooksIntegration = googleBooksIntegration;
		this.mnemeIntegration = mnemeIntegration;
		this.pluralsightIntegration = pluralsightIntegration;
	}

	public async Task ActivateSource(int id, string type, CancellationToken ct = default)
	{
		if (type == GoogleBooksSource.Type)
		{
			GoogleBooksSource source = await googleBooksIntegration.GetSource(id, ct).ConfigureAwait(false);
			source.Active = true;
			await googleBooksIntegration.UpdateSource(source, ct).ConfigureAwait(false);
		}
		else if (type == MnemeSource.Type)
		{
			MnemeSource source = await mnemeIntegration.GetSource(id, ct).ConfigureAwait(false);
			source.Active = true;
			await mnemeIntegration.UpdateSource(source, ct).ConfigureAwait(false);
		}
		else if (type == PluralsightSource.Type)
		{
			MnemeSource source = await mnemeIntegration.GetSource(id, ct).ConfigureAwait(false);
			source.Active = true;
			await mnemeIntegration.UpdateSource(source, ct).ConfigureAwait(false);
		}
	}

	public async Task<IReadOnlyList<Note>> GetNotes(bool activeOnly, CancellationToken ct = default)
	{
		Task<IReadOnlyList<GoogleBooksNote>> googleBooksTask = googleBooksIntegration.GetNotes(ct);
		Task<IReadOnlyList<MnemeNote>> mnemeTask = mnemeIntegration.GetNotes(ct);
		Task<IReadOnlyList<PluralsightNote>> pluralsightTask = pluralsightIntegration.GetNotes(ct);

		await Task.WhenAll(googleBooksTask, mnemeTask, pluralsightTask).ConfigureAwait(false);

		var ret = new List<Note>();
		ret.AddRange(await googleBooksTask);
		ret.AddRange(await mnemeTask);
		ret.AddRange(await pluralsightTask);
		return ret;
	}

	public async Task<IReadOnlyList<Source>> GetSources(bool activeOnly, CancellationToken ct = default)
	{
		Task<IReadOnlyList<GoogleBooksSource>> googleBooksTask = googleBooksIntegration.GetSources(ct);
		Task<IReadOnlyList<MnemeSource>> mnemeTask = mnemeIntegration.GetSources(ct);
		Task<IReadOnlyList<PluralsightSource>> pluralsightTask = pluralsightIntegration.GetSources(ct);

		await Task.WhenAll(googleBooksTask, mnemeTask, pluralsightTask).ConfigureAwait(false);

		var ret = new List<Source>();
		ret.AddRange(await googleBooksTask);
		ret.AddRange(await mnemeTask);
		ret.AddRange(await pluralsightTask);
		return ret;
	}
	public async Task<IReadOnlyList<Source>> GetKnownSources(bool onlyActive = true, CancellationToken ct = default)
	{
		Task<IReadOnlyList<GoogleBooksSource>> googleBooksTask = googleBooksIntegration.GetKnownSources(onlyActive, ct);
		Task<IReadOnlyList<MnemeSource>> mnemeTask = mnemeIntegration.GetKnownSources(onlyActive, ct);
		Task<IReadOnlyList<PluralsightSource>> pluralsightTask = pluralsightIntegration.GetKnownSources(onlyActive, ct);

		await Task.WhenAll(googleBooksTask, mnemeTask, pluralsightTask).ConfigureAwait(false);

		var ret = new List<Source>();
		ret.AddRange(await googleBooksTask);
		ret.AddRange(await mnemeTask);
		ret.AddRange(await pluralsightTask);
		return ret;
	}

	public async Task<IReadOnlyList<Note>> GetKnownNotes(bool onlyActive = true, CancellationToken ct = default)
	{
		Task<IReadOnlyList<GoogleBooksNote>> googleBooksTask = googleBooksIntegration.GetKnownNotes(onlyActive, ct);
		Task<IReadOnlyList<MnemeNote>> mnemeTask = mnemeIntegration.GetKnownNotes(onlyActive, ct);
		Task<IReadOnlyList<PluralsightNote>> pluralsightTask = pluralsightIntegration.GetKnownNotes(onlyActive, ct);

		await Task.WhenAll(googleBooksTask, mnemeTask, pluralsightTask).ConfigureAwait(false);

		var ret = new List<Note>();
		ret.AddRange(await googleBooksTask);
		ret.AddRange(await mnemeTask);
		ret.AddRange(await pluralsightTask);
		return ret;
	}

	public async Task IgnoreSource(int id, string type, CancellationToken ct = default)
	{
		if (type == GoogleBooksSource.Type)
		{
			GoogleBooksSource source = await googleBooksIntegration.GetSource(id, ct).ConfigureAwait(false);
			source.Active = false;
			await googleBooksIntegration.UpdateSource(source, ct).ConfigureAwait(false);
		}
		else if (type == MnemeSource.Type)
		{
			MnemeSource source = await mnemeIntegration.GetSource(id, ct).ConfigureAwait(false);
			source.Active = false;
			await mnemeIntegration.UpdateSource(source, ct).ConfigureAwait(false);
		}
		else if (type == PluralsightSource.Type)
		{
			MnemeSource source = await mnemeIntegration.GetSource(id, ct).ConfigureAwait(false);
			source.Active = false;
			await mnemeIntegration.UpdateSource(source, ct).ConfigureAwait(false);
		}
	}

	public async Task<Source> GetSource(int id, string type, CancellationToken ct)
	{
		if (type == GoogleBooksSource.Type)
			return await googleBooksIntegration.GetSource(id, ct).ConfigureAwait(false);
		else if (type == MnemeSource.Type)
		{
			return await mnemeIntegration.GetSource(id, ct).ConfigureAwait(false);
		}
		else if (type == PluralsightSource.Type)
		{
			return await mnemeIntegration.GetSource(id, ct).ConfigureAwait(false);
		}
		throw new ArgumentException("Type value didn't match to any of the source types");
	}
}
