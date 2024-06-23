using Mneme.Model.Sources;

namespace Mneme.DataAccess
{
	//TODO delete?
	public interface ISourceSaver<T> where T : Source
	{
		public bool Save(T bookSource);
	}
}
