using Mneme.Model;

namespace Mneme.Integrations.Pluralsight.Contract;

public class PluralsightNote : Note
{
	public PluralsightSource? Source { get; set; }
	public int SourceId { get; set; }
	public required string Module { get; set; }
	public required string Clip { get; set; }
	public required string TimeInClip { get; set; }
	private string pluralsightIntegrationId;
	public required string PluralsightIntegrationId { get { return pluralsightIntegrationId; } init { pluralsightIntegrationId = value; IntegrationId = PluralsightIntegrationId; } }
	public override DateTime CreationTime { get; protected set; } = DateTime.Now;
}
