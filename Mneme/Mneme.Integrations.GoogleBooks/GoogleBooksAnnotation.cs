using Google.Apis.Books.v1.Data;

namespace Mneme.Integrations.GoogleBooks;

internal class GoogleBooksAnnotation
{
	public string Id { get; private set; }
	/*		public string TextBefore { get; private set; }
            public string TextAfter { get; private set; }*/
	public DateTime Created { get; private set; }
	/// <summary>
	/// User's note
	/// </summary>
	public string UserText { get; private set; }
	public string HighlightStyle { get; private set; }
	public string Type { get; private set; }
	/// <summary>
	/// What user highlights
	/// </summary>
	public string SelectedText { get; private set; }
	public string PageId { get; private set; }
	public string Path { get; private set; }
	public DateTime LastUpdate { get; private set; }
	public string BookTitle { get; private set; }
	public string BookId { get; private set; }

	public GoogleBooksAnnotation(Annotation annotation, Volume volume)
	{
		BookId = volume.Id;
		BookTitle = volume.VolumeInfo.Title;
		//annotation.VolumeId;
		Id = annotation.Id;
		/*this.TextBefore = annotation.BeforeSelectedText;
        this.TextAfter = annotation.AfterSelectedText;*/
		Created = GoogleBooksDateTimeToDateTime(annotation.Created);
		UserText = annotation.Data;
		HighlightStyle = annotation.HighlightStyle;
		Type = annotation.Kind;
		SelectedText = annotation.SelectedText;
		PageId = PageIdsConverter(annotation.PageIds);
		//TODO check bookmarks
		//8 -> 9 wywala exception GBTextRange is null
		if (annotation.ClientVersionRanges.GbTextRange == null)
		{

		}
		//dla bookmarkow dac PageId zamiast startposition
		Path = TryConvert(annotation.ClientVersionRanges.GbTextRange?.StartPosition, volume.Id, out var path) ? path : string.Empty;
		LastUpdate = GoogleBooksDateTimeToDateTime(annotation.Updated);
	}

	private DateTime GoogleBooksDateTimeToDateTime(string created) => DateTime.Parse(created);

	private string PageIdsConverter(IList<string> pageIds)
	{
		var ret = "";
		if (pageIds != null)
		{
			foreach (var item in pageIds)
			{
				ret += item;
			}
		}
		return ret;
	}
	private bool TryConvert(string pagePath, string bookId, out string path)
	{
		path = string.Empty;
		if (pagePath is null) return false;
		if (bookId is null) return false;
		path = CreateUrlPath(bookId, pagePath);
		return true;
	}

	private string CreateUrlPath(string bookId, string pagePath)
	{
		//https://play.google.com/books/reader?id=3P3PJwAAAEAJ&pg=GBS.PA6
		var ret = @"https://play.google.com/books/reader?id=";
		ret += bookId + "&pg=" + pagePath;
		return ret;
	}
}
