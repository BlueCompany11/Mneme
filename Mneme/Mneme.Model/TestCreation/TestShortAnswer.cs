using System;

namespace Mneme.Model.TestCreation
{
	public class TestShortAnswer : IUserTest
	{
		public string NoteId { get; set; }
		public int Id { get; set; }
		public string Question { get; set; }
		public string Answer { get; set; }
		public string Hint { get; set; }
		public int Importance { get; set; }
		public DateTime Created { get; set; }
		public TestInfo TestInfo { get; set; } = new TestInfo();
	}
}
