using AutoFixture.Xunit2;
using FluentAssertions;
using Mneme.Model;
using Mneme.Notes;
using Mneme.PrismModule.Notes.ViewModels;
using Mneme.Tests.Base;
using Moq;

namespace Mneme.PrismModule.Notes.Tests;
public class NewMnemeNoteViewModelShould : BaseTest
{
	[Theory]
	[AutoDomainData]
	public async Task LoadExistingMnemeSource_WhenNavigatedTo([Frozen] Mock<IMnemeNotesProxy> proxy, NewMnemeNoteViewModel sut, IReadOnlyList<Source> sources)
	{
		_ = proxy.Setup(p => p.GetMnemeSources(default)).Returns(Task.FromResult(sources));
		sut.SourcesPreviews = [];

		sut.OnNavigatedTo(null);
		await Task.Delay(20);

		_ = sut.SourcesPreviews.Should().NotBeEmpty();
	}
}
