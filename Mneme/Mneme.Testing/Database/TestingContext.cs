using Microsoft.EntityFrameworkCore;
using Mneme.DataAccess;
using Mneme.Testing.TestCreation;

namespace Mneme.Testing.Database;

internal class TestingContext : Context
{
	public DbSet<TestShortAnswer> TestShortAnswers { get; set; }
	public DbSet<TestMultipleChoices> TestMultipleChoices { get; set; }
	public DbSet<TestMultipleChoice> TestMultipleChoice { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		_ = modelBuilder.Entity<TestShortAnswer>(entity =>
		{
			_ = entity.Property(e => e.Id).ValueGeneratedOnAdd();
			_ = entity.HasKey(e => e.Id);
			_ = entity.Property(e => e.Created).HasDefaultValueSql("CURRENT_TIMESTAMP");
		});
		_ = modelBuilder.Entity<TestMultipleChoices>(entity =>
		{
			_ = entity.Property(e => e.Id).ValueGeneratedOnAdd();
			_ = entity.HasKey(e => e.Id);
			_ = entity.HasMany(e => e.Answers);
			_ = entity.Property(e => e.Created).HasDefaultValueSql("CURRENT_TIMESTAMP");
		});
		_ = modelBuilder.Entity<TestMultipleChoice>(entity =>
		{
			_ = entity.Property(e => e.Id).ValueGeneratedOnAdd();
			_ = entity.HasKey(e => e.Id);
			_ = entity.HasOne(e => e.Test).WithMany().HasForeignKey(e => e.TestId);
		});
	}
}
