using Mneme.Model;

namespace Mneme.Integrations.GoogleBooks.Contract
{
	public class GoogleBooksSource : Source
	{
		public override string TextType => Type;

		public override string GetDetails()
		{
			return "";
		}

		public static string Type => "GoogleBooks";
	}
}
