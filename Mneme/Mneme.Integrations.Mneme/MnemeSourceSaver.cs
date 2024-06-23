using Mneme.DataAccess;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Integrations.Mneme.Database;

namespace Mneme.Integrations.Mneme
{
	public class MnemeSourceSaver : ISourceSaver<MnemeSource>
	{
		public bool Save(MnemeSource source)
		{
			using var context = new MnemeContext();
			if (context.MnemeSources.FirstOrDefault(x => x.Title == source.Title && x.Details == source.Details) != null)
				return false;
			_ = context.Add(source);
			_ = context.SaveChanges();
			return true;
		}
	}
}
