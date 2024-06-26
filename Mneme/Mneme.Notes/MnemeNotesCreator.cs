using Mneme.Core;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Model.Sources;

namespace Mneme.Notes
{
	public class MnemeNotesCreator
	{
		private readonly IBundledIntegrationFacades integration;
		private readonly IMnemeIntegrationFacade mnemeIntegration;

		public MnemeNotesCreator(IBundledIntegrationFacades integration, IMnemeIntegrationFacade mnemeIntegration)
		{
			this.integration = integration;
			this.mnemeIntegration = mnemeIntegration;
		}
		public async Task<MnemeNote> SaveMnemeNote(SourcePreview sourcePreview, string content, string title, string path, CancellationToken ct)
		{
			var newSource = (await integration.GetSource(sourcePreview.Id, sourcePreview.TypeOfSource, ct)) as MnemeSource;
			var note = new MnemeNote() { IntegrationId = Guid.NewGuid().ToString(), Content = content, Title = title, Path = path, CreationTime = DateTime.Now, Source = newSource };
			await mnemeIntegration.CreateNote(note, ct);
			return note;
		}

		public async Task<IReadOnlyList<SourcePreview>> GetSourcesPreviews(CancellationToken ct)
		{
			return (await integration.GetActiveSources()).Where(x => x.TypeToString() == MnemeSource.Type).Select(x => SourcePreview.CreateFromSource(x)).ToList();
		}
	}
}
