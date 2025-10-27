using Mneme.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mneme.Integrations.GoogleBooks.Contract;

public class GoogleBooksSource : ISource
{
	public string TextType => Type;

	public string GetDetails() => "";
	public bool IsSame(ISource other) => IntegrationId == other.IntegrationId;

	public static string Type => "GoogleBooks";

	private string? googleBooksIntegrationId;
	[NotMapped]
	public required string GoogleBooksSourceId { get => googleBooksIntegrationId; init { googleBooksIntegrationId = value; IntegrationId = googleBooksIntegrationId; } }

	public int Id { get; set; }

	public string IntegrationId {get; private set; }

	public string Title { get; set; }

	public DateTime CreationTime { get; set; }

	public bool Active { get; set; }
}
