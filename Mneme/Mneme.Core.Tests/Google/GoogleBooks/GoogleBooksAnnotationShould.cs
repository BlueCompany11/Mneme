using System;
using AutoFixture;
using FluentAssertions;
using Google.Apis.Books.v1.Data;
using Mneme.Core.Google.GoogleBooks;
using Xunit;

namespace Mneme.Core.Tests.Google.GoogleBooks
{
	public class GoogleBooksAnnotationShould
	{
		[Fact]
		public void MapPropertiesCorrectlyOnConstructor()
		{
			var fixture = new Fixture();
			var annotation = fixture.Build<Annotation>()
				.With(x => x.Created, fixture.Create<DateTime>().ToString())
				.With(x => x.Updated, fixture.Create<DateTime>().ToString())
				.Create();
			var volume = fixture.Create<Volume>();

			var sut = new GoogleBooksAnnotation(annotation, volume);

			_ = sut.Id.Should().Be(annotation.Id);
			_ = sut.Created.Should().Be(DateTime.Parse(annotation.Created));
			_ = sut.UserText.Should().Be(annotation.Data);
			_ = sut.HighlightStyle.Should().Be(annotation.HighlightStyle);
			_ = sut.Type.Should().Be(annotation.Kind);
			_ = sut.SelectedText.Should().Be(annotation.SelectedText);
			_ = sut.PageId.Should().NotBeNullOrEmpty();
			_ = sut.Path.Should().NotBeNullOrEmpty();
			_ = sut.LastUpdate.Should().Be(DateTime.Parse(annotation.Updated));
			_ = sut.BookTitle.Should().Be(volume.VolumeInfo.Title);
			_ = sut.BookId.Should().Be(volume.Id);
		}
	}
}
