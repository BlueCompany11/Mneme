using Mneme.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mneme.Integrations.GoogleBooks.Contract;

public class GoogleBooksSource : Source
{
	public override string TextType => Type;

	public override string GetDetails() => "";

	public static string Type => "GoogleBooks";

	private string? googleBooksIntegrationId;
	[NotMapped]
	public required string GoogleBooksSourceId { get => googleBooksIntegrationId; init { googleBooksIntegrationId = value; IntegrationId = googleBooksIntegrationId; } }
}
