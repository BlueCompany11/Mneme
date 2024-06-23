using System;

namespace Mneme.Model.Sources
{
	public abstract class Source
	{
		public int Id { get; set; }
		/// <summary>
		/// Used to recognize if 2 sources are the same
		/// </summary>
		public required string IntegrationId { get; set; }
		public required string Title { get; set; }
		public DateTime Created { get; } = DateTime.Now;
		public required bool Active { get; set; }
		public abstract string TypeToString();
		public abstract string GetDetails();
	}
}
