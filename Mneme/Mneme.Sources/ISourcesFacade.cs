using Mneme.Model;

namespace Mneme.Sources;
public interface ISourcesFacade
{
	Task<ISource> ActivateSource(ISource source);
	Task<IReadOnlyList<ISource>> GetKnownSourcesPreviewAsync(bool onlyActive = false, CancellationToken ct = default);
	Task<IReadOnlyList<ISource>> GetSourcesPreviewAsync(CancellationToken ct = default);
	Task<ISource> IgnoreSource(ISource source);
}