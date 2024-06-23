using Mneme.Integrations.Pluralsight.Contract;

namespace Mneme.Integrations.Pluralsight
{
	public interface IPluralsightPreelaborationProvider
	{
		List<PluralsightPreelaboration> Preelaborations { get; }
		List<PluralsightPreelaboration> Open(string path);
		bool TryOpen(string path, out List<PluralsightPreelaboration> preelaborations);
	}
}
