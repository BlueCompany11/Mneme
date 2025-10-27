using Mneme.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mneme.Integrations.GoogleBooks.Contract;

public class GoogleBooksNote : INote
{
	public required string NoteType { get; init; }
	public int SourceId { get; init; }
	public required GoogleBooksSource? Source { get; set; }
	[NotMapped]
	public required string IntegrationId { get; set; }

	[NotMapped]
	public DateTime CreationTime { get; set; }

	public int Id { get; set; }
	public required string Title { get; init; }
	public required string Path { get; init; }
	public required string Content { get; init; }
}
