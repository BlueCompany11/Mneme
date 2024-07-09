using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mneme.Core;
using Mneme.Integrations.Contracts;
using Mneme.Integrations.GoogleBooks.Contract;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Integrations.Pluralsight.Contract;
using Mneme.Model;

namespace Mneme.PrismModule.Integration.Facade
{
	public class BundledIntegrationFacades : IBundledIntegrationFacades
	{
		private readonly IIntegrationFacade<GoogleBooksSource, GoogleBooksNote> googleBooksIntegration;
		private readonly IIntegrationFacade<MnemeSource, MnemeNote> mnemeIntegration;
		private readonly IIntegrationFacade<PluralsightSource, PluralsightNote> pluralsightIntegration;
		private bool disposedValue;

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
				var source = await googleBooksIntegration.GetSource(id, ct);
				source.Active = true;
				await googleBooksIntegration.UpdateSource(source, ct);
			}
			else if (type == MnemeSource.Type)
			{
				var source = await mnemeIntegration.GetSource(id, ct);
				source.Active = true;
				await mnemeIntegration.UpdateSource(source, ct);
			}
			else if (type == PluralsightSource.Type)
			{
				var source = await mnemeIntegration.GetSource(id, ct);
				source.Active = true;
				await mnemeIntegration.UpdateSource(source, ct);
			}
		}

		public async Task<IReadOnlyList<Note>> GetNotes(bool activeOnly, CancellationToken ct = default)
		{
			var googleBooksTask = googleBooksIntegration.GetNotes(ct);
			var mnemeTask = mnemeIntegration.GetNotes(ct);
			var pluralsightTask = pluralsightIntegration.GetNotes(ct);

			await Task.WhenAll(googleBooksTask, mnemeTask, pluralsightTask);

			var ret = new List<Note>();
			ret.AddRange(await googleBooksTask);
			ret.AddRange(await mnemeTask);
			ret.AddRange(await pluralsightTask);
			return ret;
		}

		public async Task<IReadOnlyList<Source>> GetSources(bool activeOnly, CancellationToken ct = default)
		{
			var googleBooksTask = googleBooksIntegration.GetSources(ct);
			var mnemeTask = mnemeIntegration.GetSources(ct);
			var pluralsightTask = pluralsightIntegration.GetSources(ct);

			await Task.WhenAll(googleBooksTask, mnemeTask, pluralsightTask);

			var ret = new List<Source>();
			ret.AddRange(await googleBooksTask);
			ret.AddRange(await mnemeTask);
			ret.AddRange(await pluralsightTask);
			return ret;
		}
		public async Task<IReadOnlyList<Source>> GetKnownSources(bool onlyActive = true, CancellationToken ct = default)
		{
			var googleBooksTask = googleBooksIntegration.GetKnownSources(onlyActive, ct);
			var mnemeTask = mnemeIntegration.GetKnownSources(onlyActive, ct);
			var pluralsightTask = pluralsightIntegration.GetKnownSources(onlyActive, ct);

			await Task.WhenAll(googleBooksTask, mnemeTask, pluralsightTask);

			var ret = new List<Source>();
			ret.AddRange(await googleBooksTask);
			ret.AddRange(await mnemeTask);
			ret.AddRange(await pluralsightTask);
			return ret;
		}

		public async Task<IReadOnlyList<Note>> GetKnownNotes(bool onlyActive = true, CancellationToken ct = default)
		{
			var googleBooksTask = googleBooksIntegration.GetKnownNotes(onlyActive, ct);
			var mnemeTask = mnemeIntegration.GetKnownNotes(onlyActive, ct);
			var pluralsightTask = pluralsightIntegration.GetKnownNotes(onlyActive, ct);

			await Task.WhenAll(googleBooksTask, mnemeTask, pluralsightTask);

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
				var source = await googleBooksIntegration.GetSource(id, ct);
				source.Active = false;
				await googleBooksIntegration.UpdateSource(source, ct);
			}
			else if (type == MnemeSource.Type)
			{
				var source = await mnemeIntegration.GetSource(id, ct);
				source.Active = false;
				await mnemeIntegration.UpdateSource(source, ct);
			}
			else if (type == PluralsightSource.Type)
			{
				var source = await mnemeIntegration.GetSource(id, ct);
				source.Active = false;
				await mnemeIntegration.UpdateSource(source, ct);
			}
		}

		public async Task<Source> GetSource(int id, string type, CancellationToken ct)
		{
			if (type == GoogleBooksSource.Type)
				return await googleBooksIntegration.GetSource(id, ct);
			else if (type == MnemeSource.Type)
			{
				return await mnemeIntegration.GetSource(id, ct);
			}
			else if (type == PluralsightSource.Type)
			{
				return await mnemeIntegration.GetSource(id, ct);
			}
			throw new ArgumentException("Type value didn't match to any of the source types");
		}
	}
}
