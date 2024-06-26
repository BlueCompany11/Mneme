using Mneme.Model.Interfaces;
using Mneme.Model.Notes;

namespace Mneme.Integrations.Mneme.Contract
{
	public class MnemeNote : Note
	{
		public MnemeSource? Source { get; set; }

		public override INoteTest Accept(INoteTestVisitor visitor)
		{
			return visitor is INoteTestVisitor<MnemeNote> v
				? v.GetTestNote(this)
				: throw new Exception("Wrong interface for " + visitor.GetType());
		}
	}
}
