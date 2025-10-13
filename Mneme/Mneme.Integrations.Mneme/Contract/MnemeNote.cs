using Mneme.Model;

namespace Mneme.Integrations.Mneme.Contract;

public class MnemeNote : Note
{
	public MnemeSource? Source { get; set; }
	public override string IntegrationId => Title + Content;
	public override DateTime CreationTime { get; protected set; } = DateTime.Now;
}
