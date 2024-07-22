using Mneme.Model;

namespace Mneme.Integrations.Pluralsight.Contract;

public class PluralsightSource : Source
{
	private readonly string details;

	public PluralsightSource() => details = string.Empty;

	public PluralsightSource(PluralsightNote pluralsightNote) => details = pluralsightNote.Path;

	public override string GetDetails() => details;
	public override string TextType => Type;
	public static string Type => "Pluralsight";
}
