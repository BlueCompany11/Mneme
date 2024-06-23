using System.Linq;
using FluentAssertions;
using Mneme.Core.PreelaborationsProviders;
using Mneme.Integrations.Pluralsight;
using Xunit;

namespace Mneme.Core.Tests.PreelaborationsProviders
{
	public class PluralsightPreelaborationProviderShould
	{
		private readonly PluralsightPreelaborationProvider sut;
		public PluralsightPreelaborationProviderShould()
		{
			sut = new(new PluralsightNoteIdProvider());
		}
		[Fact]
		public void ReadCorrectlyFromFilePath()
		{
			var ret = sut.Open("user-notes.csv");
			_ = ret.Should().HaveCount(75);
			var x = ret.Where(x => x.TimeInClip == "2:19");
			_ = sut.Preelaborations.Should().HaveCount(75);
			_ = sut.Preelaborations.Should().ContainSingle(x =>
			x.Title == "Introduction to UML" &&
			x.Content == "Rodzaje diagramow w structural modeling" &&
			x.Clip == "Types of Modeling" &&
			x.Module == "UML Basics" &&
			x.TimeInClip == "2:19" &&
			x.Path == "https://app.pluralsight.com/player?course=uml-introduction&author=mike-erickson&name=uml-introduction-m2-basics&clip=1&mode=live&start=139&noteid=8cec0611-085c-4724-ab87-666b2e42e46f"
			);
			_ = sut.Preelaborations.Should().ContainSingle(x =>
			x.Title == "Introduction to UML" &&
			x.Content == "Rodzaje diagramow w behavioral modeling" &&
			x.Clip == "Types of Modeling" &&
			x.Module == "UML Basics" &&
			x.TimeInClip == "3:04" &&
			x.Path == "https://app.pluralsight.com/player?course=uml-introduction&author=mike-erickson&name=uml-introduction-m2-basics&clip=1&mode=live&start=184&noteid=29293bf1-b549-4a6f-aa38-42e5465740a7"
			);
			_ = sut.Preelaborations.Should().NotContain(x =>
			string.IsNullOrWhiteSpace(x.Title) ||
			string.IsNullOrWhiteSpace(x.Content) ||
			string.IsNullOrWhiteSpace(x.Clip) ||
			string.IsNullOrWhiteSpace(x.Module) ||
			string.IsNullOrWhiteSpace(x.TimeInClip) ||
			string.IsNullOrWhiteSpace(x.Path)
			);
		}
	}
}
