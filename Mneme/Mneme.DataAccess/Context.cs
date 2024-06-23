using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Mneme.DataAccess
{
	public class Context : DbContext
	{
		public Context()
		{

		}
		public Context(DbContextOptions opt) : base(opt)
		{

		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
				string mnemeFolderPath = Path.Combine(appDataPath, "Mneme");
				if (!Directory.Exists(mnemeFolderPath))
				{
					Directory.CreateDirectory(mnemeFolderPath);
				}
				string databasePath = Path.Combine(mnemeFolderPath, "Database.db");

				optionsBuilder.UseSqlite($"Data Source={databasePath}");
			}
		}

	}
}
