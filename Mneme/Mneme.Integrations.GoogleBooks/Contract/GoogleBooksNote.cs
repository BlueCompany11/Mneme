using Mneme.Model;

namespace Mneme.Integrations.GoogleBooks.Contract;

public class GoogleBooksNote : Note
{
	public required string NoteType { get; init; }
	public int SourceId { get; init; }
	public required GoogleBooksSource? Source { get; set; }
	private string googleBooksIntegrationId;
	public required string GoogleBooksNoteId { get { return googleBooksIntegrationId; } init { googleBooksIntegrationId = value; IntegrationId = googleBooksIntegrationId; } }
	public DateTime CreationDate { get { return creationDate; } init { creationDate = value; CreationTime = creationDate; } }
	private DateTime creationDate;
}
