using System.Threading;
using System.Threading.Tasks;
using Mneme.Integrations.Mneme.Contract;

namespace Mneme.Core
{
	public interface IMnemeIntegrationFacade
	{
		Task DeleteNote(string id, CancellationToken ct = default);
		Task DeleteSource(string id, CancellationToken ct = default);
		Task<bool> Create(MnemeSource source, CancellationToken ct = default);
		Task CreateNote(MnemeNote note, CancellationToken ct = default);
		Task UpdateSource(string id, string title, string details, CancellationToken ct = default);
	}
}
