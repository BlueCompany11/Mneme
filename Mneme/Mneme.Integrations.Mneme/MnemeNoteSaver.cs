using Mneme.Integrations.Mneme.Contract;
using Mneme.Integrations.Mneme.Database;

namespace Mneme.Integrations.Mneme
{
	public class MnemeNoteSaver
	{
		public void Save(MnemeNote note)
		{
			using var context = new MnemeContext();
			var source = context.MnemeSources.FirstOrDefault(x => x.IntegrationId == note.Source.IntegrationId);
			note.Source = source;
			context.MnemePreelaboration.Add(note);
			context.SaveChanges();
		}
	}
}
