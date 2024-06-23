using System;
using System.Collections.Generic;
using System.Windows;
using Mneme.Model.TestCreation;
using Mneme.Testing.UsersTests;
using Mneme.Views.Base;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Mneme.PrismModule.Testing.ViewModels.UsersTests
{
	public class TestingViewModel : BindableBase, INavigationAware
	{
		private readonly TestPreviewProvider testPreviewProvider;
		private readonly IRegionManager regionManager;

		private Queue<IUserTest> UserTests { get; set; }
		public DelegateCommand NextTestCommand { get; set; }
		private string nextTestButtonText;
		public string NextTestButtonText
		{
			get => nextTestButtonText;
			set => SetProperty(ref nextTestButtonText, value);
		}
		private bool isNextEnabled;
		public bool IsNextEnabled
		{
			get => isNextEnabled;
			set => SetProperty(ref isNextEnabled, value);
		}

		private Visibility correctButtonVisibility;
		public Visibility CorrectButtonVisibility
		{
			get => correctButtonVisibility;
			set => SetProperty(ref correctButtonVisibility, value);
		}

		private IUserTest CurrentTest { get; set; }
		public TestingViewModel(TestPreviewProvider testPreviewProvider, IRegionManager regionManager)
		{
			this.testPreviewProvider = testPreviewProvider;
			this.regionManager = regionManager;
			NextTestCommand = new DelegateCommand(NextTest);
			NextTestButtonText = "Next test";
			CorrectButtonVisibility = Visibility.Visible;
		}
		public void NextTest()
		{
			var test = CurrentTest;
			CorrectButtonVisibility = Visibility.Hidden;
			bool isNextTest = UserTests.TryPeek(out test);
			if (!isNextTest)
			{
				NextTestButtonText = "Testing finished";
				return;
			}
			CurrentTest = test;
			CurrentTest.Tested(true);
			var param = new NavigationParameters()
			{
				{ "test", UserTests.Dequeue() }
			};
			if (CurrentTest is TestShortAnswer)
			{
				regionManager.RequestNavigate(RegionNames.TestingRegion, "ShortAnswerTestView", param); //TODO
			}
			else if (CurrentTest is TestMultipleChoices)
			{
				regionManager.RequestNavigate(RegionNames.TestingRegion, "MultipleAnswersTestView", param); //TODO
			}
			else if (CurrentTest is TestClozeDeletion)
			{
				regionManager.RequestNavigate(RegionNames.TestingRegion, "ClozeDeletionTestView", param); //TODO
			}
			else
			{
				throw new Exception("No such test type " + test);
			}
		}
		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return true;
		}

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{

		}

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			UserTests = testPreviewProvider.GetTestsForToday();
		}

	}
}
