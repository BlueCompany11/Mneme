using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace Mneme.DataAccess;

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
			var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var mnemeFolderPath = Path.Combine(appDataPath, "Mneme");
			if (!Directory.Exists(mnemeFolderPath))
			{
				_ = Directory.CreateDirectory(mnemeFolderPath);
			}
			var databasePath = Path.Combine(mnemeFolderPath, "Database.db");

			_ = optionsBuilder.UseSqlite($"Data Source={databasePath}");
		}
	}

}
