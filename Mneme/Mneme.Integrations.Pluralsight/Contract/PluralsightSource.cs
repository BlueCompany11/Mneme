using Mneme.Model.Sources;

namespace Mneme.Integrations.Pluralsight.Contract
{
	public class PluralsightSource : Source
	{
		private readonly string details;

		public PluralsightSource()
		{
			details = string.Empty;
		}

		public PluralsightSource(PluralsightNote pluralsightPreelaboration)
		{
			details = pluralsightPreelaboration.Path;
		}

		public override string TypeToString()
		{
			return Type;
		}

		public override string GetDetails()
		{
			return details;
		}

		public static string Type => "Pluralsight";
	}
}
