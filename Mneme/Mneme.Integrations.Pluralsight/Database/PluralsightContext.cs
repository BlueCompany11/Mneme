using Microsoft.EntityFrameworkCore;
using Mneme.DataAccess;
using Mneme.Integrations.Pluralsight.Contract;

namespace Mneme.Integrations.Pluralsight.Database
{
	internal class PluralsightContext : Context
	{
		public DbSet<PluralsightSource> PluralsightSources { get; set; }
		public DbSet<PluralsightConfig> PluralsightConfigs { get; set; }
		public DbSet<PluralsightNote> PluralsightNotes { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PluralsightSource>(entity =>
			{
				entity.Property(e => e.Id).ValueGeneratedOnAdd();
				entity.HasKey(e => e.Id);
				entity.HasIndex(e => e.IntegrationId).IsUnique();
			});

			modelBuilder.Entity<PluralsightNote>(entity =>
			{
				entity.Property(e => e.Id).ValueGeneratedOnAdd();
				entity.HasKey(e => e.Id);
				entity.HasOne(e => e.Source);
				entity.HasIndex(e => e.IntegrationId).IsUnique();
			});
		}
	}
}
