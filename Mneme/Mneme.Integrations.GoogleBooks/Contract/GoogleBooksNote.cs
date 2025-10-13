using Mneme.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mneme.Integrations.GoogleBooks.Contract;

public class GoogleBooksNote : Note
{
	public required string NoteType { get; init; }
	public int SourceId { get; init; }
	public required GoogleBooksSource? Source { get; set; }
	private string? googleBooksIntegrationId;
	[NotMapped]
	public required string GoogleBooksNoteId { get => googleBooksIntegrationId; init { googleBooksIntegrationId = value; IntegrationId = googleBooksIntegrationId; } }

	[NotMapped]
	public DateTime CreationDate { get => creationDate; init { creationDate = value; CreationTime = creationDate; } }
	private DateTime creationDate;
}
