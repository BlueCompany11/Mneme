using Mneme.Integrations.GoogleBooks.Contract;

namespace Mneme.Integrations.GoogleBooks
{
	internal class GoogleBooksAnnotationToPreelaborationMapper
	{
		public GoogleBooksPreelaboration Convert(GoogleBooksAnnotation annotation)
		{
			return new GoogleBooksPreelaboration
			{
				Content = annotation.SelectedText,
				CreationTime = annotation.Created,
				NoteType = annotation.Type,
				Path = annotation.Path,
				Title = annotation.BookTitle,
				IntegrationId = annotation.Id,
				Source = new GoogleBooksSource { Title = annotation.BookTitle, IntegrationId = annotation.BookId, Active = true }
			};
		}
	}
}
