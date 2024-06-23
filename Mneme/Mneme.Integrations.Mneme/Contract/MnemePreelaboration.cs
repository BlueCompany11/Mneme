using Mneme.Model.Interfaces;
using Mneme.Model.Preelaborations;

namespace Mneme.Integrations.Mneme.Contract
{
	public class MnemePreelaboration : Preelaboration
	{
		public MnemeSource? Source { get; set; }

		public override INoteTest Accept(INoteTestVisitor visitor)
		{
			return visitor is INoteTestVisitor<MnemePreelaboration> v
				? v.GetTestNote(this)
				: throw new Exception("Wrong interface for " + visitor.GetType());
		}
	}
}
