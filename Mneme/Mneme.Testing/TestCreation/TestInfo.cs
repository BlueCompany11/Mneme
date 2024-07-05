using System;

namespace Mneme.Model.TestCreation
{
	public class TestInfo
	{
		public int Id { get; set; }
		public DateTime Updated { get; set; } = DateTime.Now;
		public int Occurrence { get; set; } = 0;
	}
}
