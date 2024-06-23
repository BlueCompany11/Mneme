using AutoFixture;
using FluentAssertions;
using Google.Apis.Books.v1.Data;
using Mneme.Core.Google.GoogleBooks;
using Xunit;

namespace Mneme.Core.Tests.Google.GoogleBooks
{
	public class GoogleBooksBookShould
	{
		[Fact]
		public void MapCorrectlyPropertiesOnConstructor()
		{
			var fixture = new Fixture();
			var volume = fixture.Create<Volume>();

			var sut = new GoogleBooksBook(volume);

			_ = sut.Id.Should().Be(volume.Id);
			_ = sut.Title.Should().Be(volume.VolumeInfo.Title);
			_ = sut.Path.Should().Be(volume.AccessInfo.WebReaderLink);
			_ = sut.BookShelveName.Should().BeNullOrEmpty();
		}

		[Fact]
		public void MapCorrectlyPropertiesOnSecondConstructor()
		{
			var fixture = new Fixture();
			var volume = fixture.Create<Volume>();
			var bookShelf = fixture.Create<Bookshelf>();
			var sut = new GoogleBooksBook(volume, bookShelf);

			_ = sut.Id.Should().Be(volume.Id);
			_ = sut.Title.Should().Be(volume.VolumeInfo.Title);
			_ = sut.Path.Should().Be(volume.AccessInfo.WebReaderLink);
			_ = sut.BookShelveName.Should().Be(bookShelf.Title);
		}

	}
}
