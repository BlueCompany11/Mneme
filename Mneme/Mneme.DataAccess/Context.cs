using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace Mneme.DataAccess;

public class Context : DbContext
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (optionsBuilder.IsConfigured)
			return;

		var mnemeFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Mneme");
		_ = Directory.CreateDirectory(mnemeFolder);
		var dbPath = Path.Combine(mnemeFolder, "Database.db");
		_ = optionsBuilder.UseSqlite($"Data Source={dbPath}");
	}
}
