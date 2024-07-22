using MaterialDesignThemes.Wpf;
using Mneme.Model;
using Mneme.PrismModule.Integration.Facade;
using Mneme.Views.Base;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;

namespace Mneme.PrismModule.Testing.ViewModels.TestCreation;

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

	private Note CurrentNote { get; set; }

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
		var uri = value == TestOptions[0]
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
				{"note", CurrentNote }
			};
		regionManager.RequestNavigate("TestPickRegion", uri, param);
	}

	public void OnNavigatedTo(NavigationContext navigationContext)
	{
		CurrentNote = navigationContext.Parameters.GetValue<Note>("note");
		ChangeTestOptionView(SelectedTestOption);
		navigator.NavigateToPreview(CurrentNote, navigationContext.Parameters, RegionNames.NotePreviewRegion);
	}

	public bool IsNavigationTarget(NavigationContext navigationContext) => true;

	public void OnNavigatedFrom(NavigationContext navigationContext)
	{

	}
}
