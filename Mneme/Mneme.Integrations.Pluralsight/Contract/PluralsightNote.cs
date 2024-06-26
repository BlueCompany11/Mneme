using Mneme.Model.Interfaces;
using Mneme.Model.Notes;

namespace Mneme.Integrations.Pluralsight.Contract
{
	public class PluralsightNote : Note
	{
		public PluralsightSource? Source { get; set; }
		public int SourceId { get; set; }
		public required string Module { get; set; }
		public required string Clip { get; set; }
		public required string TimeInClip { get; set; }

		public override INoteTest Accept(INoteTestVisitor visitor)
		{
			return visitor is INoteTestVisitor<PluralsightNote> v
				? v.GetTestNote(this)
				: throw new Exception("Wrong interface for " + visitor.GetType());
		}
	}
}
