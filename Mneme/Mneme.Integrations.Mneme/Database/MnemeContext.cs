using Microsoft.EntityFrameworkCore;
using Mneme.DataAccess;
using Mneme.Integrations.Mneme.Contract;

namespace Mneme.Integrations.Mneme.Database
{
	public class MnemeContext : Context
	{
		public DbSet<MnemeSource> MnemeSources { get; set; }
		public DbSet<MnemePreelaboration> MnemePreelaboration { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<MnemeSource>(entity =>
			{
				entity.Property(e => e.Id).ValueGeneratedOnAdd();
				entity.HasKey(e => e.Id);
				entity.HasIndex(e => e.IntegrationId).IsUnique();
			});

			modelBuilder.Entity<MnemePreelaboration>(entity =>
			{
				entity.Property(e => e.Id).ValueGeneratedOnAdd();
				entity.HasKey(e => e.Id);
				entity.HasOne(e => e.Source);
				entity.HasIndex(e => e.IntegrationId).IsUnique();
			});
		}
	}
}
