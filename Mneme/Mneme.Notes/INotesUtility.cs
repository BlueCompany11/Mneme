using Mneme.Model;

namespace Mneme.Notes;
public interface INotesUtility
{
	Task<IReadOnlyList<INote>> GetNotes(CancellationToken ct);
}