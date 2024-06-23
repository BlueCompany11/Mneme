using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mneme.Integrations.Contracts;
using Mneme.Integrations.GoogleBooks.Contract;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Integrations.Pluralsight.Contract;
using Mneme.Model.Preelaborations;
using Mneme.Model.Sources;

namespace Mneme.Core.Interfaces
{
	public class BundledIntegrationFacades : IBundledIntegrationFacades
	{
		private readonly IIntegrationFacade<GoogleBooksSource, GoogleBooksPreelaboration> googleBooksIntegration;
		private readonly IIntegrationFacade<MnemeSource, MnemePreelaboration> mnemeIntegration;
		private readonly IIntegrationFacade<PluralsightSource, PluralsightPreelaboration> pluralsightIntegration;
		private readonly IDatabase testingModule;
		private bool disposedValue;

		public BundledIntegrationFacades(
			IIntegrationFacade<GoogleBooksSource, GoogleBooksPreelaboration> googleBooksIntegration,
			IIntegrationFacade<MnemeSource, MnemePreelaboration> mnemeIntegration,
			IIntegrationFacade<PluralsightSource, PluralsightPreelaboration> pluralsightIntegration
			)
		{
			this.googleBooksIntegration = googleBooksIntegration;
			this.mnemeIntegration = mnemeIntegration;
			this.pluralsightIntegration = pluralsightIntegration;
			//this.testingModule = testingModule;
		}
		public async Task ActivateSource(string id, string type, CancellationToken ct = default)
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
		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task DeleteNote(string id, CancellationToken ct = default)
		{
			await mnemeIntegration.DeleteNote(id, ct);
		}
		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="id"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public async Task DeleteSource(string id, CancellationToken ct = default)
		{
			await mnemeIntegration.DeleteSource(id, ct);
		}

		public async Task<IReadOnlyList<Preelaboration>> GetActiveNotes(CancellationToken ct = default)
		{
			var ret = new List<Preelaboration>();
			ret.AddRange(await googleBooksIntegration.GetNotes(ct));
			ret.AddRange(await mnemeIntegration.GetNotes(ct));
			ret.AddRange(await pluralsightIntegration.GetNotes(ct));
			return ret;
			//TODO
			//throw new NotImplementedException();
			//return ret.Where(x=>x.Active).ToList();
		}

		public async Task<IReadOnlyList<Source>> GetActiveSources(CancellationToken ct = default)
		{
			var ret = new List<Source>();
			ret.AddRange(await googleBooksIntegration.GetSources(ct));
			ret.AddRange(await mnemeIntegration.GetSources(ct));
			ret.AddRange(await pluralsightIntegration.GetSources(ct));
			return ret.Where(x => x.Active).ToList();
		}
		/// <summary>
		/// Not yet fully implemented
		/// </summary>
		/// <param name="ct"></param>
		/// <returns></returns>
		public async Task<IReadOnlyList<Preelaboration>> GetNotes(CancellationToken ct = default)
		{
			return await GetActiveNotes(ct);
		}

		public async Task<IReadOnlyList<Source>> GetSources(CancellationToken ct = default)
		{
			var ret = new List<Source>();
			ret.AddRange(await googleBooksIntegration.GetSources(ct));
			ret.AddRange(await mnemeIntegration.GetSources(ct));
			ret.AddRange(await pluralsightIntegration.GetSources(ct));
			return ret;
		}
		public async Task<IReadOnlyList<Source>> GetKnownSources(bool onlyActive, CancellationToken ct = default)
		{
			var ret = new List<Source>();
			ret.AddRange(await googleBooksIntegration.GetKnownSources(onlyActive, ct));
			ret.AddRange(await mnemeIntegration.GetKnownSources(onlyActive, ct));
			ret.AddRange(await pluralsightIntegration.GetKnownSources(onlyActive, ct));
			return ret;
		}
		/// <summary>
		/// Only Mneme
		/// </summary>
		public async Task IgnoreSource(string id, string type, CancellationToken ct = default)
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

		public async Task MigrateDatabase(CancellationToken ct = default)
		{
			await googleBooksIntegration.MigrateDatabase(ct);
			await mnemeIntegration.MigrateDatabase(ct);
			await pluralsightIntegration.MigrateDatabase(ct);
			//await testingModule.MigrateDatabase(ct); //TODO
		}
		/// <summary>
		/// Only Mneme
		/// </summary>
		public async Task UpdateSource(string id, string title, string details, CancellationToken ct = default)
		{
			var source = await mnemeIntegration.GetSource(id, ct);
			source.Title = title;
			source.Details = details;
			await mnemeIntegration.UpdateSource(source, ct);
		}

		public async Task<bool> Create(MnemeSource source, CancellationToken ct = default)
		{
			//work around to problem where integration id's were different but for some reason db was telling it already exists
			try
			{
				var sourceDb = await mnemeIntegration.GetSource(source.IntegrationId, ct);
				return false;
			}
			catch (InvalidOperationException) 
			{
				await mnemeIntegration.CreateSource(source);
				return true;
			}
		}

		public async Task CreateNote(MnemePreelaboration note, CancellationToken ct = default)
		{
			await mnemeIntegration.CreateNote(note);
		}

		public async Task<Source> GetSource(string id, string type, CancellationToken ct)
		{
			if (type == GoogleBooksSource.Type)
			{
				return await googleBooksIntegration.GetSource(id, ct);
			}
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

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
					googleBooksIntegration.Dispose();
					mnemeIntegration.Dispose();
					pluralsightIntegration.Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				disposedValue = true;
			}
		}

		// // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		// ~BundledIntegrationFacades()
		// {
		//     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		//     Dispose(disposing: false);
		// }

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
