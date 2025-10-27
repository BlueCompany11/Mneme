using Mneme.Model;

namespace Mneme.Integrations.Mneme.Contract;

public class MnemeSource : ISource
{
	public int Id { get; set; }
	/// <summary>
	/// Used to recognize if 2 sources are the same
	/// </summary>
	public required string Title { get; set; }
	public required DateTime CreationTime { get; init; }
	public required bool Active { get; set; }
	public string IntegrationId => $"{Title} {Details}";
	public string TextType => Type;
	public string GetDetails() => Details ?? string.Empty;
	public string? Details { get; set; }
	public static string Type => "Mneme";
	public bool IsSame(ISource other) => IntegrationId == other.IntegrationId;
}
