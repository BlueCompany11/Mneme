using Mneme.Integrations.Mneme.Contract;
using Mneme.Model;

namespace Mneme.Notes;
public interface IMnemeNotesProxy
{
	Task DeleteNote(Note note);
	Task<IReadOnlyList<ISource>> GetMnemeSources(CancellationToken ct);
	Task<MnemeNote> SaveMnemeNote(ISource source, string content, string title, string path, CancellationToken ct);
}