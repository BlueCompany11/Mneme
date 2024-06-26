using Microsoft.EntityFrameworkCore;
using Mneme.DataAccess;
using Mneme.Integrations.Mneme.Contract;

namespace Mneme.Integrations.Mneme.Database
{
	internal class MnemeContext : Context
	{
		public DbSet<MnemeSource> MnemeSources { get; set; }
		public DbSet<MnemeNote> MnemeNotes { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<MnemeSource>(entity =>
			{
				entity.Property(e => e.Id).ValueGeneratedOnAdd();
				entity.HasKey(e => e.Id);
				entity.HasIndex(e => e.IntegrationId).IsUnique();
			});

			modelBuilder.Entity<MnemeNote>(entity =>
			{
				entity.Property(e => e.Id).ValueGeneratedOnAdd();
				entity.HasKey(e => e.Id);
				entity.HasOne(e => e.Source);
				entity.HasIndex(e => e.IntegrationId).IsUnique();
			});
		}
	}
}
