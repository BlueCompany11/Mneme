using Mneme.Model.Notes;
using Prism.Events;

namespace Mneme.Desktop.Events
{
	public class SelectedNoteChanged : PubSubEvent<Note>
	{
	}
}
