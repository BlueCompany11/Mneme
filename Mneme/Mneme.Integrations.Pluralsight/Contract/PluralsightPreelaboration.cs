using Mneme.Model.Interfaces;
using Mneme.Model.Preelaborations;

namespace Mneme.Integrations.Pluralsight.Contract
{
	public class PluralsightPreelaboration : Preelaboration
	{
		public PluralsightSource? Source { get; set; }
		public int SourceId { get; set; }
		public required string Module { get; set; }
		public required string Clip { get; set; }
		public required string TimeInClip { get; set; }

		public override INoteTest Accept(INoteTestVisitor visitor)
		{
			return visitor is INoteTestVisitor<PluralsightPreelaboration> v
				? v.GetTestNote(this)
				: throw new Exception("Wrong interface for " + visitor.GetType());
		}
	}
}
