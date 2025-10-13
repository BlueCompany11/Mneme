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

		//code duplicated 1
		var mnemeFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Mneme");
		//end code duplicated 1
		var dbPath = Path.Combine(mnemeFolder, "Database.db");
		_=optionsBuilder.UseSqlite($"Data Source={dbPath}");
	}
}
