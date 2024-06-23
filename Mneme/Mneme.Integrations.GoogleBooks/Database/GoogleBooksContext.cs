using Microsoft.EntityFrameworkCore;
using Mneme.DataAccess;
using Mneme.Integrations.GoogleBooks.Contract;

namespace Mneme.Integrations.GoogleBooks.Database
{
	public class GoogleBooksContext : Context
	{
		public DbSet<GoogleBooksSource> GoogleBooksSources { get; set; }
		public DbSet<GoogleBooksPreelaboration> GoogleBooksPreelaborations { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<GoogleBooksSource>(entity =>
			{
				entity.Property(e => e.Id).ValueGeneratedOnAdd();
				entity.HasKey(e => e.Id);
				entity.HasIndex(e => e.IntegrationId).IsUnique();
			});

			modelBuilder.Entity<GoogleBooksPreelaboration>(entity =>
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
