using System;

namespace Mneme.Model;

public interface INote
{
	public int Id { get; }
	/// <summary>
	/// Used to recognize if 2 notes are the same
	/// </summary>
	public string IntegrationId { get; }
	public string Title { get; }
	public string Path { get; }
	public DateTime CreationTime { get; }
	public string Content { get; }
}
