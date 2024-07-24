using AutoFixture.Xunit2;
using Mneme.Model;
using Mneme.Tests.Base;
using Moq;
using Prism.Regions;
using Mneme.PrismModule.Sources.ViewModels;
using Mneme.Sources;
using FluentAssertions;
using System.Collections.ObjectModel;

namespace Mneme.PrismModule.Sources.Tests;

public class SourcesViewModelShould : BaseTest
{
	[Theory]
	[AutoDomainData]
	public async Task LoadAllSources_WhenNavigatedTo([Frozen] Mock<ISourcesFacade> facade, SourcesViewModel sut, IReadOnlyList<Source> sources, Mock<NavigationContext> nav)
	{
		facade.Setup(x => x.GetSourcesPreviewAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(sources));
		sut.AllItems.Clear();

		sut.OnNavigatedTo(nav.Object);
		await Task.Delay(20);

		sut.AllItems.Should().NotBeEmpty();
	}

	[Theory]
	[AutoDomainData]
	public async Task StopsLoadingNotes_WhenNavigatedFrom_AndLoadedPreviouslyDataWasNotEmpty([Frozen] Mock<ISourcesFacade> facade, SourcesViewModel sut, IReadOnlyList<Source> sources, Mock<NavigationContext> nav)
	{
		var oldSources = CreateMany<Source>();
		facade.Setup(x => x.GetSourcesPreviewAsync(It.IsAny<CancellationToken>())).Returns(async () => {
			await Task.Delay(TimeSpan.FromSeconds(5));
			return sources;
		});
		sut.AllItems.AddRange(oldSources);

		sut.OnNavigatedTo(nav.Object);
		sut.OnNavigatedFrom(nav.Object);
		await Task.Delay(20);

		sut.AllItems.Count.Should().BeGreaterThanOrEqualTo(oldSources.Count);
	}

	[Theory]
	[AutoDomainData]
	public async Task LoadsNotes_WhenNavigatedFrom_AndLoadedPreviouslyDataWasEmpty([Frozen] Mock<ISourcesFacade> facade, SourcesViewModel sut, IReadOnlyList<Source> sources, Mock<NavigationContext> nav)
	{
		facade.Setup(x => x.GetSourcesPreviewAsync(It.IsAny<CancellationToken>())).Returns(async () => {
			await Task.Delay(TimeSpan.FromMilliseconds(100));
			return sources;
		});
		sut.AllItems.Clear();

		sut.OnNavigatedTo(nav.Object);
		sut.OnNavigatedFrom(nav.Object);
		await Task.Delay(150);

		sut.AllItems.Count.Should().Be(sources.Count);
	}
}
