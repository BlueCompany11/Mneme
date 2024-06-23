using Mneme.Core.Interfaces;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Model.Sources;

namespace Mneme.Notes
{
	public class MnemeNotesCreator
	{
		private readonly IBundledIntegrationFacades integration;

		public MnemeNotesCreator(IBundledIntegrationFacades integration)
		{
			this.integration = integration;
		}
		public async Task<MnemePreelaboration> SaveMnemeNote(SourcePreview sourcePreview, string content, string title, string path, CancellationToken ct)
		{
			var newSource = (await integration.GetSource(sourcePreview.Id, sourcePreview.TypeOfSource, ct)) as MnemeSource;
			var note = new MnemePreelaboration() { IntegrationId = Guid.NewGuid().ToString(), Content = content, Title = title, Path = path, CreationTime = DateTime.Now, Source = newSource };
			await integration.CreateNote(note, ct);
			return note;
		}

		public async Task<IReadOnlyList<SourcePreview>> GetSourcesPreviews(CancellationToken ct)
		{
			return (await integration.GetActiveSources()).Where(x => x.TypeToString() == MnemeSource.Type).Select(x => SourcePreview.CreateFromSource(x)).ToList();
		}
	}
}
