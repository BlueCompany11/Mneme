namespace Mneme.Model.Interfaces
{
	public interface INoteTestVisitor
	{

	}
	public interface INoteTest
	{

	}

	public interface INoteTestVisitor<T>
	{
		INoteTest GetTestNote(T preelaboration);
	}
}
