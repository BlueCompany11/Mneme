using System;

namespace Mneme.Model;

public abstract class Note
{
	public int Id { get; set; }
	/// <summary>
	/// Used to recognize if 2 notes are the same
	/// </summary>
	public virtual string IntegrationId { get; protected set; }
	public required string Title { get; init; }
	public required string Path { get; init; }
	public virtual DateTime CreationTime { get; protected set; }
	public required string Content { get; init; }
}
