using Microsoft.EntityFrameworkCore;
using Mneme.DataAccess;
using Mneme.Integrations.GoogleBooks.Contract;

namespace Mneme.Integrations.GoogleBooks.Database
{
	internal class GoogleBooksContext : Context
	{
		public DbSet<GoogleBooksSource> GoogleBooksSources { get; set; }
		public DbSet<GoogleBooksNote> GoogleBooksNotes { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<GoogleBooksSource>(entity =>
			{
				entity.Property(e => e.Id).ValueGeneratedOnAdd();
				entity.HasKey(e => e.Id);
				entity.HasIndex(e => e.IntegrationId).IsUnique();
			});

			modelBuilder.Entity<GoogleBooksNote>(entity =>
			{
				entity.Property(e => e.Id).ValueGeneratedOnAdd();
				entity.HasKey(e => e.Id);
				entity.HasOne(e => e.Source)
				 .WithMany()
				 .HasForeignKey(e => e.SourceId);
				entity.HasIndex(e => e.IntegrationId).IsUnique();
			});
		}
	}
}
