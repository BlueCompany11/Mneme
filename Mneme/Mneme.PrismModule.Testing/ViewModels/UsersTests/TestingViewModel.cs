﻿using System.Collections.Generic;
using System.Windows;
using Mneme.Model.TestCreation;
using Mneme.PrismModule.Testing.Views.UsersTests;
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

		private bool isNextEnabled;
		public bool IsNextEnabled
		{
			get => isNextEnabled;
			set => SetProperty(ref isNextEnabled, value);
		}

		private bool finishedTesting;
		public bool FinishedTesting
		{
			get => finishedTesting;
			set => SetProperty(ref finishedTesting, value);
		}

		private IUserTest CurrentTest { get; set; }
		public TestingViewModel(TestPreviewProvider testPreviewProvider, IRegionManager regionManager)
		{
			this.testPreviewProvider = testPreviewProvider;
			this.regionManager = regionManager;
			NextTestCommand = new DelegateCommand(NextTest);
		}
		public void NextTest()
		{
			var test = CurrentTest;
			
			bool isNextTest = UserTests.TryPeek(out test);
			if (!isNextTest)
			{
				FinishedTesting = true;
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
				regionManager.RequestNavigate(RegionNames.TestingRegion, nameof(ShortAnswerTestView), param);
			}
			else if (CurrentTest is TestMultipleChoices)
			{
				regionManager.RequestNavigate(RegionNames.TestingRegion, nameof(MultipleAnswersTestView), param);
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
			NextTest();
		}

	}
}
