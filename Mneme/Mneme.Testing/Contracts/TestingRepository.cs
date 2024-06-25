using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mneme.Model.Sources;
using Mneme.Model.TestCreation;
using Mneme.Testing.Database;

namespace Mneme.Testing.Contracts
{
	public class TestingRepository
	{
		public void CreateTest(TestClozeDeletion test)
		{
			using var context = new TestingContext();
			context.Add(test);
			context.SaveChanges();
		}
		public void CreateTest(TestMultipleChoices test)
		{
			using var context = new TestingContext();
			context.Add(test);
			context.SaveChanges();
		}
		public void CreateTest(TestShortAnswer test) 
		{
			using var context = new TestingContext();
			context.Add(test);
			context.SaveChanges();
		}
	}
}
