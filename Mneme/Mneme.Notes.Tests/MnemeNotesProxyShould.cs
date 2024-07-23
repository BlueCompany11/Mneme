using AutoFixture;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using Mneme.Core;
using Mneme.Integrations.Contracts;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Model;
using Mneme.Tests.Base;
using Moq;

namespace Mneme.Notes.Tests;

public class MnemeNotesProxyShould : BaseTest
{
    [Fact]
    public async Task SaveMnemeNote_ShouldReturnCorrectlyInitializedNote()
    {
        var source = fixture.Create<MnemeSource>();
        var integration = Mock.Of<IIntegrationFacade<MnemeSource, MnemeNote>>(x =>
            x.GetSource(source.Id, default) == Task.FromResult(source) &&
            x.CreateNote(It.IsAny<MnemeNote>()) == Task.CompletedTask);
        var sut = new MnemeNotesProxy(integration);

		var note = await sut.SaveMnemeNote(source, "content", "title", "path", default);


		Mock.Get(integration).Verify(x => x.CreateNote(It.Is<MnemeNote>(n =>
			n.Source == source &&
			n.Content == "content" &&
			n.Title == "title" &&
			n.Path == "path")));

		note.Source.Should().Be(source);
		note.CreationTime.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
		note.IntegrationId.Should().Be("titlecontent");

    }
}
