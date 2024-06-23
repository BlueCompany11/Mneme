using Mneme.DataAccess;
using Mneme.Integrations.Contracts;
using Mneme.Integrations.Mneme.Database;

namespace Mneme.Integrations.Mneme.Contract
{
	public class MnemeSourcesProvider : BaseSourcesProvider<MnemeSource>
	{
		public MnemeSourcesProvider(Context sourcesContext)
		{
		}

		protected override void AddSources(List<MnemeSource> sources)
		{
			using var context = new MnemeContext();
			context.MnemeSources.AddRange(sources);
			context.SaveChanges();
		}

		protected override Task<List<MnemeSource>> GetSourcesFromAccountAsync(CancellationToken ct)
		{
			return Task.FromResult(new List<MnemeSource>());
		}

		protected override List<MnemeSource> GetSourcesFromDatabase()
		{
			using var context = new MnemeContext();
			return context.MnemeSources.ToList();
		}

		protected override bool AreSame(MnemeSource note1, MnemeSource note2)
		{
			return note1.IntegrationId == note2.IntegrationId;
		}
	}
}
