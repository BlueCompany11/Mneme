namespace Mneme.Integrations.Pluralsight
{
	public interface IPluralsightConfigProvider
	{
		PluralsightConfig Config { get; }
		void UpdatePath(string path);
		event Action SourceUpdated;
	}
}
