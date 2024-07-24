using Mneme.Integrations.Mneme.Contract;
using Mneme.Model;

namespace Mneme.Notes;
public interface IMnemeNotesProxy
{
	Task DeleteNote(Note note);
	Task<IReadOnlyList<Source>> GetMnemeSources(CancellationToken ct);
	Task<MnemeNote> SaveMnemeNote(Source source, string content, string title, string path, CancellationToken ct);
}