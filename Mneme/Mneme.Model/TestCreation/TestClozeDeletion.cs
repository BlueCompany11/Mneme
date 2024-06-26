﻿using System;
using System.Collections.Generic;

namespace Mneme.Model.TestCreation
{
	public class TestClozeDeletion : IUserTest
	{
		public string NoteId { get; set; }
		public int Id { get; set; }
		public string Text { get; set; }
		public int Importance { get; set; }
		public List<ClozeDeletionDataStructure> ClozeDeletionDataStructures { get; set; }
		public DateTime Created { get; set; }
		public TestInfo TestInfo { get; set; } = new TestInfo();
	}
}
