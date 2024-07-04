using Microsoft.EntityFrameworkCore;
using Mneme.DataAccess;
using Mneme.Model.TestCreation;

namespace Mneme.Testing.Database
{
	internal class TestingContext : Context
	{
		public DbSet<TestShortAnswer> TestShortAnswers { get; set; }
		public DbSet<TestMultipleChoices> TestMultipleChoices { get; set; }
		public DbSet<TestClozeDeletion> TestClozeDeletions { get; set; }
		public DbSet<TestInfo> TestInfos { get; set; }
		public DbSet<TestMultipleChoice> TestMultipleChoice { get; set; }
	}
}
