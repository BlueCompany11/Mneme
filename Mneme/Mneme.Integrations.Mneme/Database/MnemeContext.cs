using Microsoft.EntityFrameworkCore;
using Mneme.DataAccess;
using Mneme.Integrations.Mneme.Contract;

namespace Mneme.Integrations.Mneme.Database;

internal class MnemeContext : Context
{
	public DbSet<MnemeSource> MnemeSources { get; set; }
	public DbSet<MnemeNote> MnemeNotes { get; set; }
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		_ = modelBuilder.Entity<MnemeSource>(entity =>
		{
			_ = entity.Property(e => e.Id).ValueGeneratedOnAdd();
			_ = entity.HasKey(e => e.Id);
			_ = entity.Ignore(e => e.IntegrationId);
			_ = entity.Property(e => e.CreationTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
		});

		_ = modelBuilder.Entity<MnemeNote>(entity =>
		{
			_ = entity.Property(e => e.Id).ValueGeneratedOnAdd();
			_ = entity.HasKey(e => e.Id);
			_ = entity.HasOne(e => e.Source);
			_ = entity.Ignore(e => e.IntegrationId);
			_ = entity.Property(e => e.CreationTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
		});

	}
}
