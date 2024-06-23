using AutoFixture;
using FluentAssertions;
using Mneme.Core.Visitors;
using Mneme.Integrations.Pluralsight;
using Xunit;

namespace Mneme.Core.Tests
{
	public class PreelaborationPreviewVisitorShould
	{
		[Fact]
		public void GiveAllPropertiesToReturnedObjectFromGoogleBooksPreelaboration()
		{
			Fixture fixture = new();
			var data = fixture.Create<GoogleBooksPreelaboration>();
			var sut = new PreelaborationPreviewVisitor();
			var ret = sut.GetPreelaborationPreview(data);
			_ = ret.Title.Should().NotBeEmpty();
			_ = ret.Date.Should().NotBe(default);
			_ = ret.Id.Should().NotBe(default);
			_ = ret.Note.Should().NotBeEmpty();
			//TODO
			//ret.Tags.Should().NotBeEmpty();
			_ = ret.Preelaboration.Should().NotBeNull();
		}
		[Fact]
		public void GiveAllPropertiesToReturnedObjectFromPluralsightPreelaboration()
		{
			Fixture fixture = new();
			var data = fixture.Create<PluralsightPreelaboration>();
			var sut = new PreelaborationPreviewVisitor();
			var ret = sut.GetPreelaborationPreview(data);
			_ = ret.Title.Should().NotBeEmpty();
			_ = ret.Date.Should().NotBe(default);
			_ = ret.Id.Should().NotBe(default);
			_ = ret.Note.Should().NotBeEmpty();
			//TODO
			//ret.Tags.Should().NotBeEmpty();
			_ = ret.Preelaboration.Should().NotBeNull();
		}
	}
}
