using AutoFixture.Xunit2;
using FluentAssertions;
using Mneme.Integrations.Contracts;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Tests.Base;
using Moq;

namespace Mneme.Notes.Tests;

public class MnemeNotesProxyShould : BaseTest
{
	[Theory]
	[AutoDomainData]
	public async Task SaveMnemeNote_ShouldReturnCorrectlyInitializedNote(MnemeSource source, [Frozen] Mock<IIntegrationFacade<MnemeSource, MnemeNote>> integration, MnemeNotesProxy sut)
	{
		_ = integration.Setup(x => x.GetSource(source.Id, default)).Returns(Task.FromResult(source));
		_ = integration.Setup(x => x.CreateNote(It.IsAny<MnemeNote>())).Returns(Task.CompletedTask);

		var note = await sut.SaveMnemeNote(source, "content", "title", "path", default);

		_ = note.Source.Should().Be(source);
		_ = note.CreationTime.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
		_ = note.IntegrationId.Should().Be("titlecontent");

	}
}
