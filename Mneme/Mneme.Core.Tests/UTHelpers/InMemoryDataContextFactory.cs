using Microsoft.EntityFrameworkCore;
using Mneme.DataAccess;

namespace Mneme.Core.Tests.UTHelpers
{
	internal class InMemoryDataContextFactory
	{
		private DbContextOptionsBuilder builder;
		private Context context;
		public Context Build(string databaseName)
		{
			builder = new DbContextOptionsBuilder();
			_ = builder.UseInMemoryDatabase(databaseName);
			context = new Context(builder.Options);
			return context;
		}
	}
}
