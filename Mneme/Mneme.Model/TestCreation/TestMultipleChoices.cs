using System;
using System.Collections.Generic;

namespace Mneme.Model.TestCreation
{
	public class TestMultipleChoices : IUserTest
	{
		public string NoteId { get; set; }
		public int Id { get; set; }
		public string Question { get; set; }
		public List<TestMultipleChoice> Answers { get; set; } = new ();
		public int Importance { get; set; }
		public DateTime Created { get; set; }
		public TestInfo TestInfo { get; set; } = new TestInfo();
	}
}
