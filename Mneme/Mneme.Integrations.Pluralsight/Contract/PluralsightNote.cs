using Mneme.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mneme.Integrations.Pluralsight.Contract;

public class PluralsightNote : INote
{
	public PluralsightSource? Source { get; set; }
	public int SourceId { get; set; }
	public required string Module { get; set; }
	public required string Clip { get; set; }
	public required string TimeInClip { get; set; }
	[NotMapped]
	public required string IntegrationId { get; set; }
	public DateTime CreationTime { get; protected set; } = DateTime.Now;

	public int Id { get; set; }

	public string Title { get; set; }

	public string Path { get; set; }

	public string Content { get; set; }
}
