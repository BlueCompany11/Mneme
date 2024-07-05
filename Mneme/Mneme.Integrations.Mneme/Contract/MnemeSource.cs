using Mneme.Model.Sources;

namespace Mneme.Integrations.Mneme.Contract
{
	public class MnemeSource : Source
	{
		public static string GenerateIntegrationId(string title, string details) => $"{title} {details}";
		public override string TextType => Type;

		public override string GetDetails()
		{
			return Details ?? string.Empty;
		}

		public string? Details { get; set; }
		public static string Type => "Mneme";
	}
}
