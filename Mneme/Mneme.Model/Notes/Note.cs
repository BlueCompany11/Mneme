using System;
using Mneme.Model.Interfaces;

namespace Mneme.Model.Preelaborations
{
	public abstract class Note
	{
		public int Id { get; set; }
		/// <summary>
		/// Used to recognize if 2 notes are the same
		/// </summary>
		public required string IntegrationId { get; init; }
		public required string Title { get; init; }
		public required string Path { get; init; }
		public DateTime CreationTime { get; init; }
		public required string Content { get; init; }
		public abstract INoteTest Accept(INoteTestVisitor visitor);

	}
}
