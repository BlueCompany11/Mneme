using Mneme.Core;
using Mneme.Integrations.Contracts;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Model.Sources;

namespace Mneme.Notes
{
	public class MnemeNotesCreator
	{
		private readonly IIntegrationFacade<MnemeSource, MnemeNote> mnemeIntegration;

		public MnemeNotesCreator(IIntegrationFacade<MnemeSource, MnemeNote> mnemeIntegration)
		{
			this.mnemeIntegration = mnemeIntegration;
		}
		public async Task<MnemeNote> SaveMnemeNote(SourcePreview sourcePreview, string content, string title, string path, CancellationToken ct)
		{
			var newSource = (await mnemeIntegration.GetSource(sourcePreview.Id, ct));
			var note = new MnemeNote() { IntegrationId = Guid.NewGuid().ToString(), Content = content, Title = title, Path = path, CreationTime = DateTime.Now, Source = newSource };
			await mnemeIntegration.CreateNote(note);
			return note;
		}

		public async Task<IReadOnlyList<SourcePreview>> GetSourcesPreviews(CancellationToken ct)
		{
			return (await mnemeIntegration.GetActiveSources(ct)).Where(x => x.TypeToString() == MnemeSource.Type).Select(x => SourcePreview.CreateFromSource(x)).ToList();
		}
	}
}
