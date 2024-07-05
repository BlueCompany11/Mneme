using Mneme.Model.Notes;

namespace Mneme.Integrations.Mneme.Contract
{
	public class MnemeNote : Note
	{
		public MnemeSource? Source { get; set; }
	}
}
