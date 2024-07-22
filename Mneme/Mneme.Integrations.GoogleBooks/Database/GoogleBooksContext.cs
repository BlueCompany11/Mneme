using Microsoft.EntityFrameworkCore;
using Mneme.DataAccess;
using Mneme.Integrations.GoogleBooks.Contract;

namespace Mneme.Integrations.GoogleBooks.Database;

internal class GoogleBooksContext : Context
{
	public DbSet<GoogleBooksSource> GoogleBooksSources { get; set; }
	public DbSet<GoogleBooksNote> GoogleBooksNotes { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		_ = modelBuilder.Entity<GoogleBooksSource>(entity =>
		{
			_ = entity.Property(e => e.Id).ValueGeneratedOnAdd();
			_ = entity.HasKey(e => e.Id);
			_ = entity.HasIndex(e => e.IntegrationId).IsUnique();
		});

		_ = modelBuilder.Entity<GoogleBooksNote>(entity =>
		{
			_ = entity.Property(e => e.Id).ValueGeneratedOnAdd();
			_ = entity.HasKey(e => e.Id);
			_ = entity.HasOne(e => e.Source)
			 .WithMany()
			 .HasForeignKey(e => e.SourceId);
			_ = entity.HasIndex(e => e.IntegrationId).IsUnique();
		});
	}
}
