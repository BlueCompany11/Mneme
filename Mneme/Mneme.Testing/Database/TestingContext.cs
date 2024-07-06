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
		public DbSet<ClozeDeletionDataStructure> ClozeDeletionDataStructure { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<TestShortAnswer>(entity =>
			{
				entity.Property(e => e.Id).ValueGeneratedOnAdd();
				entity.HasKey(e => e.Id);
				entity.HasOne(e => e.TestInfo);
			});
			modelBuilder.Entity<TestMultipleChoices>(entity =>
			{
				entity.Property(e => e.Id).ValueGeneratedOnAdd();
				entity.HasKey(e => e.Id);
				entity.HasOne(e => e.TestInfo);
				entity.HasMany(e => e.Answers);
			});
			modelBuilder.Entity<TestMultipleChoice>(entity =>
			{
				entity.Property(e => e.Id).ValueGeneratedOnAdd();
				entity.HasKey(e => e.Id);
				entity.HasOne(e => e.Test).WithMany().HasForeignKey(e => e.TestId);
			});
			modelBuilder.Entity<TestInfo>(entity =>
			{
				entity.Property(e => e.Id).ValueGeneratedOnAdd();
				entity.HasKey(e => e.Id);
			});
			modelBuilder.Entity<TestClozeDeletion>(entity =>
			{
				entity.Property(e => e.Id).ValueGeneratedOnAdd();
				entity.HasKey(e => e.Id);
				entity.HasOne(e => e.TestInfo);
				entity.HasMany(e => e.ClozeDeletionDataStructures);
			});
		}
	}
}
