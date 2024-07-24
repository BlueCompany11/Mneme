using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
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
	[Theory]
	[AutoDomainData]
	public async Task LoadExistingMnemeSource_WhenNavigatedTo([Frozen] Mock<IMnemeNotesProxy> proxy, NewMnemeNoteViewModel sut, IReadOnlyList<Source> sources)
	{
		proxy.Setup(p => p.GetMnemeSources(default)).Returns(Task.FromResult(sources));
		sut.SourcesPreviews = [];

		sut.OnNavigatedTo(null);
		await Task.Delay(20);

		sut.SourcesPreviews.Should().NotBeEmpty();
	}
}
