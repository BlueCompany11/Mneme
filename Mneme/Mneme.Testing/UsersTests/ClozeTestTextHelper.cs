using Mneme.Model.TestCreation;

namespace Mneme.Testing.UsersTests
{
	public class ClozeTestTextHelper
	{
		public string GetTextAsQuestion(TestClozeDeletion test)
		{
			string ret = test.Text;
			for (int i = 0 ; i < test.ClozeDeletionDataStructures.Count ; i++)
			{
				var cloze = test.ClozeDeletionDataStructures[i];
				int length = cloze.End - cloze.Start;
				ret = ret.Remove(cloze.Start, length).Insert(cloze.Start, new string('_', length));
			}
			return ret;
		}
		public List<string> GetTextAsAnser(TestClozeDeletion test)
		{
			var ret = new List<string>();
			for (int i = 0 ; i < test.ClozeDeletionDataStructures.Count ; i++)
			{
				ret.Add(test.Text[test.ClozeDeletionDataStructures[i].Start..test.ClozeDeletionDataStructures[i].End]);
			}
			return ret;
		}
	}
}
