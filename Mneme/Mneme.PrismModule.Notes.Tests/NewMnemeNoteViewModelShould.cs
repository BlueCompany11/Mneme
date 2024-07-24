using AutoFixture;
using AutoFixture.AutoMoq;
using Example;
using FluentAssertions;
using Mneme.Integrations.Contracts;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Model;
using Mneme.Notes;
using Mneme.PrismModule.Notes.ViewModels;
using Mneme.Tests.Base;
using Moq;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Mneme.PrismModule.Notes.Tests;
public class NewMnemeNoteViewModelShould : BaseTest
{
	[Fact]
	public async Task LoadExistingMnemeSource_WhenNavigatedTo()
	{
		var fixture = new Fixture().Customize(new AutoMoqCustomization());
		var proxy = fixture.Freeze<Mock<IMnemeNotesProxy>>();
		IReadOnlyList<Source> sources = fixture.CreateMany<Source>().ToImmutableList();
		proxy.Setup(p => p.GetMnemeSources(default)).Returns(Task.FromResult(sources));
		var sut = fixture.Create<NewMnemeNoteViewModel>();
		sut.SourcesPreviews = [];

		sut.OnNavigatedTo(fixture.Create<NavigationContext>());
		await Task.Delay(20);

		sut.SourcesPreviews.Should().NotBeEmpty();
	}
}
