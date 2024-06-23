using Mneme.DataAccess;
using Mneme.Integrations.Contracts;
using Mneme.Integrations.GoogleBooks.Database;

namespace Mneme.Integrations.GoogleBooks.Contract
{
	public class GoogleBooksIntegrationFacade : IntegrationFacadeBase<Context, GoogleBooksSource, GoogleBooksPreelaboration>
	{
		private readonly GoogleBooksPreelaborationProvider noteProvider;

		public GoogleBooksIntegrationFacade(GoogleBooksPreelaborationProvider noteProvider) : base()
		{
			this.noteProvider = noteProvider;
		}

		protected override Context CreateContext()
		{
			return new GoogleBooksContext();
		}
		public override async Task<IReadOnlyList<GoogleBooksPreelaboration>> GetNotes(CancellationToken ct)
		{
			return await noteProvider.GetPreelaborationsAsync(ct);
		}
	}
}

