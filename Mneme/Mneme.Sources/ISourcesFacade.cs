using Mneme.Model;

namespace Mneme.Sources;
public interface ISourcesFacade
{
	Task<Source> ActivateSource(Source source);
	Task<IReadOnlyList<Source>> GetKnownSourcesPreviewAsync(bool onlyActive = false, CancellationToken ct = default);
	Task<IReadOnlyList<Source>> GetSourcesPreviewAsync(CancellationToken ct = default);
	Task<Source> IgnoreSource(Source source);
}