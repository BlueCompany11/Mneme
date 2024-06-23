using Mneme.Integrations.Pluralsight.Contract;

namespace Mneme.Integrations.Pluralsight
{
	public class PluralsightPreelaborationProvider : IPluralsightPreelaborationProvider
	{
		public List<PluralsightPreelaboration> Preelaborations { get; private set; }

		private readonly PluralsightNoteIdProvider pluralsightNoteIdProvider;

		public PluralsightPreelaborationProvider(PluralsightNoteIdProvider pluralsightNoteIdProvider)
		{
			Preelaborations = [];
			this.pluralsightNoteIdProvider = pluralsightNoteIdProvider;
		}
		public List<PluralsightPreelaboration> Open(string path)
		{
			var ret = new List<PluralsightPreelaboration>();
			if (string.IsNullOrEmpty(path))
				return ret;
			Preelaborations.Clear();
			using (var reader = new StreamReader(path))
			{
				_ = reader.ReadLine();

				while (!reader.EndOfStream)
				{
					string line = reader.ReadLine();
					string separator = @""",""";
					string[] values = line.Split(separator);
					//remove "
					values[0] = values[0][1..];
					values[5] = values[5][..^1];
					ret.Add(BuildFromCsvLine(values));
				}
			}
			Preelaborations = ret;
			return ret;
		}

		private PluralsightPreelaboration BuildFromCsvLine(string[] values)
		{
			var ret = new PluralsightPreelaboration
			{
				Content = values[0],
				Title = values[1],
				CreationTime = DateTime.Today,
				Path = values[5],
				Module = values[2],
				TimeInClip = values[4],
				Clip = values[3],
				Source = new PluralsightSource { Title = values[1], IntegrationId = values[1], Active = true },
				IntegrationId = pluralsightNoteIdProvider.GetNoteId(values[5])
			};
			return ret;
		}

		public bool TryOpen(string path, out List<PluralsightPreelaboration> preelaborations)
		{
			if (File.Exists(path))
			{
				preelaborations = Open(path);
				return true;
			}
			preelaborations = null;
			return false;
		}
	}
}
