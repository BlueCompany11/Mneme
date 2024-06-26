using System;
using System.Collections.Generic;
using MaterialDesignThemes.Wpf;
using Mneme.Integrations.GoogleBooks.Contract;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Integrations.Pluralsight.Contract;
using Mneme.Model.Preelaborations;
using Mneme.PrismModule.Integration.Facade;
using Mneme.Views.Base;
using Prism.Mvvm;
using Prism.Regions;

namespace Mneme.PrismModule.Testing.ViewModels.TestCreation
{
	public class TestCreationViewModel : BindableBase, INavigationAware
	{
		private readonly IRegionManager regionManager;
		private readonly NoteToPreviewNavigator navigator;
		private List<string> testOptions;
		public ISnackbarMessageQueue SnackbarMessageQueue { get; }
		public List<string> TestOptions
		{
			get => testOptions;
			set => SetProperty(ref testOptions, value);
		}
		private string selectedTestOption;
		public string SelectedTestOption
		{
			get => selectedTestOption;
			set
			{
				if (selectedTestOption != value)
					ChangeTestOptionView(value);
				_ = SetProperty(ref selectedTestOption, value);
			}
		}

		private Preelaboration CurrentPreelaboration { get; set; }

		public TestCreationViewModel(IRegionManager regionManager, ISnackbarMessageQueue snackbarMessageQueue, NoteToPreviewNavigator navigator)
		{
			this.regionManager = regionManager;
			SnackbarMessageQueue = snackbarMessageQueue;
			this.navigator = navigator;
			TestOptions = ["Short Answer", "Multiple Choice", "Cloze Deletion"];
			SelectedTestOption = TestOptions[0];
		}

		private void ChangeTestOptionView(string value)
		{
			string uri = value == TestOptions[0]
				? "ShortAnswerTestCreationView"
				: value == TestOptions[1]
					? "MultipleChoiceTestCreationView"
					: value == TestOptions[2] ? "ClozeDeletionTestCreationView" : throw new Exception("No such test option can be created");
			Navigate(uri);
		}

		private void Navigate(string uri)
		{
			var param = new NavigationParameters
			{
				{"pre", CurrentPreelaboration }
			};
			regionManager.RequestNavigate("TestPickRegion", uri, param);
		}

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			CurrentPreelaboration = navigationContext.Parameters.GetValue<Preelaboration>("pre");
			ChangeTestOptionView(SelectedTestOption);
			navigator.NavigateToPreview(CurrentPreelaboration, navigationContext.Parameters, RegionNames.NotePreviewRegion);
		}

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return true;
		}

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{

		}
	}
}
