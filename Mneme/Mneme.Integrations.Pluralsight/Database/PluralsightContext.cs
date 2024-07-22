using Microsoft.EntityFrameworkCore;
using Mneme.DataAccess;
using Mneme.Integrations.Pluralsight.Contract;

namespace Mneme.Integrations.Pluralsight.Database;

internal class PluralsightContext : Context
{
	public DbSet<PluralsightSource> PluralsightSources { get; set; }
	public DbSet<PluralsightConfig> PluralsightConfigs { get; set; }
	public DbSet<PluralsightNote> PluralsightNotes { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		_ = modelBuilder.Entity<PluralsightSource>(entity =>
		{
			_ = entity.Property(e => e.Id).ValueGeneratedOnAdd();
			_ = entity.HasKey(e => e.Id);
			_ = entity.HasIndex(e => e.IntegrationId).IsUnique();
			_ = entity.Property(e => e.CreationTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
		});

		_ = modelBuilder.Entity<PluralsightNote>(entity =>
		{
			_ = entity.Property(e => e.Id).ValueGeneratedOnAdd();
			_ = entity.HasKey(e => e.Id);
			_ = entity.HasOne(e => e.Source);
			_ = entity.HasIndex(e => e.IntegrationId).IsUnique();
			_ = entity.Property(e => e.CreationTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
		});
	}
}
