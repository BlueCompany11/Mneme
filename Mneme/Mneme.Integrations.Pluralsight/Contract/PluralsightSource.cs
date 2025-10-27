using Mneme.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mneme.Integrations.Pluralsight.Contract;

public class PluralsightSource : ISource
{
	private readonly string details;

	public PluralsightSource() => details = string.Empty;

	public PluralsightSource(PluralsightNote pluralsightNote) => details = pluralsightNote.Path;

	public string GetDetails() => details;
	public bool IsSame(ISource other) => other.IntegrationId == IntegrationId;

	public string TextType => Type;
	public static string Type => "Pluralsight";
	private string pluralsightIntegrationId;
	[NotMapped]
	public required string PluralsightSourceId { get => pluralsightIntegrationId; init { pluralsightIntegrationId = value; IntegrationId = pluralsightIntegrationId; } }

	public int Id { get; set; }

	public string IntegrationId { get; private set; }

	public string Title { get; set; }

	public DateTime CreationTime { get; set; }

	public bool Active { get; set; }
}
