using AutoFixture.Xunit2;
using FluentAssertions;
using Mneme.Model;
using Mneme.Notes;
using Mneme.PrismModule.Notes.ViewModels;
using Mneme.Tests.Base;
using Moq;
using Prism.Navigation.Regions;
using System.Collections.ObjectModel;

namespace Mneme.PrismModule.Notes.Tests;
public class NotesViewModelShould : BaseTest
{
	[Theory]
	[AutoDomainData]
	public async Task LoadAllNotes_WhenNavigatedTo([Frozen] Mock<INotesUtility> utility, NotesViewModel sut, IReadOnlyList<Note> notes, Mock<NavigationContext> nav)
	{
		_ = utility.Setup(x => x.GetNotes(It.IsAny<CancellationToken>())).Returns(Task.FromResult(notes));
		sut.AllItems.Clear();

		sut.OnNavigatedTo(nav.Object);
		await Task.Delay(20);

		_ = sut.AllItems.Should().NotBeEmpty();
	}

	[Theory]
	[AutoDomainData]
	public async Task StopsLoadingNotes_WhenNavigatedFrom_AndLoadedPreviouslyDataWasNotEmpty([Frozen] Mock<INotesUtility> utility, NotesViewModel sut, IReadOnlyList<Note> notes, Mock<NavigationContext> nav)
	{
		var oldNotes = CreateMany<Note>();
		_ = utility.Setup(x => x.GetNotes(It.IsAny<CancellationToken>())).Returns(async () =>
		{
			await Task.Delay(TimeSpan.FromSeconds(5));
			return notes;
		});
		_ = sut.AllItems.AddRange(oldNotes);

		sut.OnNavigatedTo(nav.Object);
		sut.OnNavigatedFrom(nav.Object);
		await Task.Delay(20);

		_ = sut.AllItems.Count.Should().BeGreaterThanOrEqualTo(oldNotes.Count);
	}

	[Theory]
	[AutoDomainData]
	public async Task LoadsNotes_WhenNavigatedFrom_AndLoadedPreviouslyDataWasEmpty([Frozen] Mock<INotesUtility> utility, NotesViewModel sut, IReadOnlyList<Note> notes, Mock<NavigationContext> nav)
	{
		_ = utility.Setup(x => x.GetNotes(It.IsAny<CancellationToken>())).Returns(async () =>
		{
			await Task.Delay(TimeSpan.FromMilliseconds(100));
			return notes;
		});
		sut.AllItems.Clear();

		sut.OnNavigatedTo(nav.Object);
		sut.OnNavigatedFrom(nav.Object);
		await Task.Delay(150);

		_ = sut.AllItems.Count.Should().Be(notes.Count);
	}
}
