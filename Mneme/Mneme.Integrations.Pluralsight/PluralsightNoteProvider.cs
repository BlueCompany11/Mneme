using Mneme.Integrations.Pluralsight.Contract;

namespace Mneme.Integrations.Pluralsight;

internal class PluralsightNoteProvider
{
	public List<PluralsightNote> Notes { get; private set; }

	public PluralsightNoteProvider() => Notes = [];
	public List<PluralsightNote> Open(string path)
	{
		var ret = new List<PluralsightNote>();
		if (string.IsNullOrEmpty(path))
			return ret;
		Notes.Clear();
		try
		{
			using (var reader = new StreamReader(path))
			{
				_ = reader.ReadLine();

				while (!reader.EndOfStream)
				{
					var line = reader.ReadLine();
					var separator = @""",""";
					var values = line.Split(separator);
					//remove "
					values[0] = values[0][1..];
					values[5] = values[5][..^1];
					ret.Add(BuildFromCsvLine(values));
				}
			}
			Notes = ret;
			return ret;
		}
		catch (Exception)
		{
			return ret;
		}
	}

	private PluralsightNote BuildFromCsvLine(string[] values)
	{
		var ret = new PluralsightNote
		{
			Content = values[0],
			Title = values[1],
			CreationTime = DateTime.Today,
			Path = values[5],
			Module = values[2],
			TimeInClip = values[4],
			Clip = values[3],
			Source = new PluralsightSource { Title = values[1], IntegrationId = values[1], Active = true },
			IntegrationId = GetNoteId(values[5])
		};
		return ret;
	}

	public bool TryOpen(string path, out List<PluralsightNote>? notes)
	{
		if (File.Exists(path))
		{
			notes = Open(path);
			return true;
		}
		notes = null;
		return false;
	}
	private static string GetNoteId(string url) => url.Split("&noteid=")[1];
}
