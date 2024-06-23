using AutoFixture;
using FluentAssertions;
using Mneme.Core.Visitors;
using Mneme.Integrations.Pluralsight;
using Mneme.Model.NotePreviews;
using Xunit;

namespace Mneme.Core.Tests.Visitors
{
	public class NotePreviewVisitorShould
	{
		private readonly Fixture fixture;
		private readonly NotePreviewVisitor sut;
		public NotePreviewVisitorShould()
		{
			fixture = new();
			sut = new NotePreviewVisitor();
		}
		[Fact]
		public void MapGoogleBooksPreelaboration()
		{
			var pre = fixture.Create<GoogleBooksPreelaboration>();
			var ret = sut.GetNotePreview(pre) as DefaultNotePreview;
			_ = ret.Title.Should().Be(pre.Title);
			_ = ret.LastEdited.Should().Be(pre.CreationTime.ToString());
			_ = ret.Link.Should().Be(pre.Path);
			_ = ret.CreationDate.Should().Be(pre.CreationTime.ToString());
			_ = ret.SourceType.Should().Be("Google Books");
			_ = ret.NoteText.Should().Be(pre.Content);
			_ = ret.Type.Should().Be(pre.NoteType);
		}
		[Fact]
		public void MapPluralsightPreelaboration()
		{
			var pre = fixture.Create<PluralsightPreelaboration>();
			var ret = sut.GetNotePreview(pre) as PluralsightNotePreview;
			_ = ret.Title.Should().Be(pre.Title);
			_ = ret.Link.Should().Be(pre.Path);
			_ = ret.Clip.Should().Be(pre.Clip);
			_ = ret.Module.Should().Be(pre.Module);
			_ = ret.Type.Should().Be("Note");
			_ = ret.TimeInClip.Should().Be(pre.TimeInClip);
			_ = ret.Note.Should().Be(pre.Content);
		}
	}
}
