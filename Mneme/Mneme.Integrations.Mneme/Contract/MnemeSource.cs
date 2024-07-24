using Mneme.Model;

namespace Mneme.Integrations.Mneme.Contract;

public class MnemeSource : Source
{
	public override string IntegrationId { get => $"{Title} {Details}"; }
	public override string TextType => Type;

	public override string GetDetails() => Details ?? string.Empty;

	public string? Details { get; set; }
	public static string Type => "Mneme";
}
