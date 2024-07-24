using Mneme.Model;

namespace Mneme.Notes;
public interface INotesUtility
{
	Task<IReadOnlyList<Note>> GetNotes(CancellationToken ct);
}