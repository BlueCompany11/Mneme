using Microsoft.EntityFrameworkCore;
using Mneme.DataAccess;
using Mneme.Model.TestCreation;

namespace Mneme.Testing.Database
{
	internal class TestingContext : Context
	{
		public DbSet<TestShortAnswer> TestShortAnswers { get; set; }
		public DbSet<TestMultipleChoices> TestMultipleChoices { get; set; }
		public DbSet<TestInfo> TestInfos { get; set; }
		public DbSet<TestMultipleChoice> TestMultipleChoice { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<TestShortAnswer>(entity =>
			{
				entity.Property(e => e.Id).ValueGeneratedOnAdd();
				entity.HasKey(e => e.Id);
				entity.HasOne(e => e.TestInfo);
				entity.Property(e => e.Created).HasDefaultValueSql("CURRENT_TIMESTAMP");
			});
			modelBuilder.Entity<TestMultipleChoices>(entity =>
			{
				entity.Property(e => e.Id).ValueGeneratedOnAdd();
				entity.HasKey(e => e.Id);
				entity.HasOne(e => e.TestInfo);
				entity.HasMany(e => e.Answers);
				entity.Property(e => e.Created).HasDefaultValueSql("CURRENT_TIMESTAMP");
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
		}
	}
}
