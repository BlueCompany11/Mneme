using System;

namespace Mneme.Model;

public interface ISource
{
	int Id { get; }
	/// <summary>
	/// Used to recognize if 2 sources are the same
	/// </summary>
	string IntegrationId { get; }
	string Title { get; set; }
	DateTime CreationTime { get; }
	bool Active { get; set; }
	string GetDetails();
	string TextType { get; }
	bool IsSame(ISource other);
}
