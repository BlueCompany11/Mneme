namespace Mneme.Testing.TestCreation;

public class TestImportanceMapper
{
	public List<string> ImportanceOptions { get; }

	private readonly string low;
	private readonly string medium;
	private readonly string high;
	public TestImportanceMapper()
	{
		low = "Low";
		medium = "Medium";
		high = "High";
		ImportanceOptions = [low, medium, high];

	}
	public int Map(string importance)
	{
		var ret = importance == low
			? 0
			: importance == medium ? 1 : importance == high ? 2 : throw new Exception("Cannot map test importance " + importance);
		return ret;
	}

	public string Map(int importance)
	{
		var ret = importance == 0
			? low
			: importance == 1 ? medium : importance == 2 ? high : throw new Exception("Cannot map test importance " + importance);
		return ret;
	}
}
