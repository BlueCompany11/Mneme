namespace Mneme.Integrations.Pluralsight
{
	public class PluralsightNoteIdProvider
	{
		public string GetNoteId(string url)
		{
			return url.Split("&noteid=")[1];
		}
	}
}
