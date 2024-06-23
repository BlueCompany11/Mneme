using System;

namespace Mneme.Model.Sources
{
	public class SourcePreview
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string TypeOfSource { get; set; }
		public DateTime Date { get; set; }
		public bool IsActive { get; set; }
		public string Details { get; set; }
		public static SourcePreview CreateFromSource(Source source)
		{
			return new SourcePreview { Id = source.IntegrationId, Title = source.Title, TypeOfSource = source.TypeToString(), Date = source.Created, IsActive = source.Active, Details = source.GetDetails() };
		}
	}
}
