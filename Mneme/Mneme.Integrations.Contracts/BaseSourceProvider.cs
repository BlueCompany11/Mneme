using Mneme.Model.Sources;

namespace Mneme.Integrations.Contracts
{
	public abstract class BaseSourcesProvider<T> where T : Source
	{
		protected abstract Task<List<T>> GetSourcesFromAccountAsync(CancellationToken ct);

		protected abstract List<T> GetSourcesFromDatabase();
		protected List<T> FindUnique(List<T> fromAccount, List<T> fromDatabase)
		{
			var existingSources = new List<T>();
			foreach (var item in fromAccount)
			{
				for (int i = 0 ; i < fromDatabase.Count ; i++)
				{
					if (AreSame(item, fromDatabase[i]))
						existingSources.Add(item);
				}
			}
			return fromAccount.Except(existingSources).ToList();
		}
		protected abstract bool AreSame(T note1, T note2);
		protected abstract void AddSources(List<T> sources);

		public async Task<List<Source>> GetSourcesAsync(bool onlyActive, CancellationToken ct)
		{
			var ret = new List<Source>();
			var fromAccount = await GetSourcesFromAccountAsync(ct);
			var fromDatabase = GetSourcesFromDatabase();
			var unique = FindUnique(fromAccount, fromDatabase);
			AddSources(unique);
			ret.AddRange(unique);
			ret.AddRange(fromDatabase);
			if (onlyActive)
				ret = ret.Where(x => x.Active).ToList();
			return ret;
		}

		public async Task<List<Source>> GetKnownSourcesAsync(bool onlyActive, CancellationToken cancellationToken)
		{
			var ret = new List<Source>();
			var fromDatabase = GetSourcesFromDatabase();
			if (onlyActive)
				fromDatabase = fromDatabase.Where(x => x.Active).ToList();
			ret.AddRange(fromDatabase);
			return ret;
		}
	}
}
