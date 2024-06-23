using Microsoft.EntityFrameworkCore;
using Mneme.DataAccess;
using Mneme.Integrations.Contracts;
using Mneme.Integrations.Mneme.Database;

namespace Mneme.Integrations.Mneme.Contract
{
	public class MnemeIntegrationFacade : IntegrationFacadeBase<Context, MnemeSource, MnemePreelaboration>
	{

		public MnemeIntegrationFacade() : base()
		{
		}

		protected override Context CreateContext()
		{
			return new MnemeContext();
		}
		public override async Task<IReadOnlyList<MnemePreelaboration>> GetNotes(CancellationToken ct)
		{
			var notes = await context.Set<MnemePreelaboration>().Include(x => x.Source).ToListAsync(ct);
			return notes;
		}
	}
}
