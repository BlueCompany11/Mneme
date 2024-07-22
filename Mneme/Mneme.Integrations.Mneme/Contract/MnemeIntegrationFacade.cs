using Microsoft.EntityFrameworkCore;
using Mneme.DataAccess;
using Mneme.Integrations.Contracts;
using Mneme.Integrations.Mneme.Database;

namespace Mneme.Integrations.Mneme.Contract;

public class MnemeIntegrationFacade : IntegrationFacadeBase<Context, MnemeSource, MnemeNote>
{

	public MnemeIntegrationFacade() : base()
	{
	}

	protected override Context CreateContext() => new MnemeContext();
	public override async Task<IReadOnlyList<MnemeNote>> GetNotes(CancellationToken ct)
	{
		using Context context = CreateContext();
		return await context.Set<MnemeNote>().Include(x => x.Source).ToListAsync(ct).ConfigureAwait(false);
	}
}
