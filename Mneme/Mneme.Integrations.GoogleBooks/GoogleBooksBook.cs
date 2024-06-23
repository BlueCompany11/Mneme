using Google.Apis.Books.v1.Data;

namespace Mneme.Integrations.GoogleBooks
{
	internal class GoogleBooksBook
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string Path { get; set; }

		public GoogleBooksBook(Volume volume)
		{
			Id = volume.Id;
			Title = volume.VolumeInfo.Title;
			Path = volume.AccessInfo.WebReaderLink;
		}
	}
}
