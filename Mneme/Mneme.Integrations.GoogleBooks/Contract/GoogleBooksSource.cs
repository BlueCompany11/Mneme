using Mneme.Model.Sources;

namespace Mneme.Integrations.GoogleBooks.Contract
{
	public class GoogleBooksSource : Source
	{
		public override string TypeToString()
		{
			return Type;
		}

		public override string GetDetails()
		{
			return "";
		}

		public static string Type => "Google Books";
	}
}
