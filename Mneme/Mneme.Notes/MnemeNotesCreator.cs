using Mneme.Integrations.Contracts;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Model;

namespace Mneme.Notes
{
	public class MnemeNotesCreator
	{
		private readonly IIntegrationFacade<MnemeSource, MnemeNote> mnemeIntegration;

		public MnemeNotesCreator(IIntegrationFacade<MnemeSource, MnemeNote> mnemeIntegration)
		{
			this.mnemeIntegration = mnemeIntegration;
		}
		public async Task<MnemeNote> SaveMnemeNote(Source source, string content, string title, string path, CancellationToken ct)
		{
			var newSource = (await mnemeIntegration.GetSource(source.Id, ct));
			var note = new MnemeNote() { IntegrationId = Guid.NewGuid().ToString(), Content = content, Title = title, Path = path, CreationTime = DateTime.Now, Source = newSource };
			await mnemeIntegration.CreateNote(note);
			return note;
		}

		public async Task<IReadOnlyList<Source>> GetSourcesPreviews(CancellationToken ct)
		{
			return (await mnemeIntegration.GetActiveSources(ct)).ToList();
		}
	}
}
