using System;
using System.Collections.Generic;
using AutoFixture;
using FluentAssertions;
using Mneme.Core;
using Mneme.Desktop.ViewModels;
using Mneme.Model;
using Mneme.Model.Interfaces;
using Mneme.Model.Preelaborations;
using Moq;
using Prism.Regions;
using Xunit;

namespace Mneme.Desktop.Tests
{
	public class NotesViewModelShould
	{
		NotesViewModel sut;
		Fixture fixture;
		Mock<IRegionManager> regionManager;
		Mock<IPreelaborationProviderMother> preelaborationsProvider;
		Mock<IPreelaborationPreviewVisitor<PluralsightPreelaboration>> preelaborationVisitor;

		public NotesViewModelShould()
		{
			fixture = new();
			regionManager = new Mock<IRegionManager>();
			preelaborationsProvider = new Mock<IPreelaborationProviderMother>();
			preelaborationVisitor = new Mock<IPreelaborationPreviewVisitor>().As<IPreelaborationPreviewVisitor<PluralsightPreelaboration>>();
			var preelaborations = fixture.Create<List<PluralsightPreelaboration>>();
			var pre = new List<Preelaboration>();
			foreach (var item in preelaborations)
			{
				pre.Add(item);
			}
			sut = new NotesViewModel(regionManager.Object, preelaborationsProvider.Object, (IPreelaborationPreviewVisitor)preelaborationVisitor.Object);
		}
		[Fact]
		public void GetAllPreelaborationsIfProviderAlreadyHasSome()
		{
			var regionManager = new Mock<IRegionManager>();
			var preelaborationsProvider = new Mock<IPreelaborationProviderMother>();
			var preelaborationVisitor = new Mock<IPreelaborationPreviewVisitor>().As<IPreelaborationPreviewVisitor<PluralsightPreelaboration>>();
			//populate with preelaborations
			var preelaborations = fixture.Create<List<PluralsightPreelaboration>>();
			var pre = new List<Preelaboration>();
			foreach (var item in preelaborations)
			{
				pre.Add(item);
			}
			//
			preelaborationsProvider.Setup(x => x.GetPreelaborations()).Returns(pre);
			
			sut = new NotesViewModel(regionManager.Object, preelaborationsProvider.Object, (IPreelaborationPreviewVisitor)preelaborationVisitor.Object);
			sut.PreelaborationsPreview.Should().HaveCount(preelaborationsProvider.Object.GetPreelaborations().Count);
		}
		[Fact]
		public void NavigateWhenSelectedPreelaborationChanges()
		{
			//sut.SelectedPreelaborationPreview = fixture.Create<>
			//var sut = new NotesViewModel();
		}
		[Fact]
		public void UpdatePreelaborationsIfNewSourceEventIsTriggered()
		{

		}
	}
}
