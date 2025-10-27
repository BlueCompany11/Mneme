using Mneme.Model;

namespace Mneme.Integrations.Mneme.Contract;

public class MnemeNote : INote
{
	public MnemeSource? Source { get; set; }
	public string IntegrationId  => Title + Content;
	public DateTime CreationTime { get; set; } = DateTime.Now;
	public int Id { get; set; }
	public string Title { get; init; }
	public string Path { get; init; }
	public string Content { get; init; }
}
