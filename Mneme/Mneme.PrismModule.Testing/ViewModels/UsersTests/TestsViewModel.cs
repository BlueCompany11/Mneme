﻿using Mneme.PrismModule.Testing.Views.TestCreation;
using Mneme.PrismModule.Testing.Views.UsersTests;
using Mneme.Testing.Contracts;
using Mneme.Testing.UsersTests;
using Mneme.Views.Base;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace Mneme.PrismModule.Testing.ViewModels.UsersTests;

public class TestsViewModel : SearchableViewModel<TestDataPreview>, INavigationAware
{
	private readonly TestPreviewProvider testPreviewProvider;
	private readonly IRegionManager regionManager;
	private readonly IDialogService dialogService;
	private readonly TestTypeProvider testTypeProvider;
	private readonly TestingRepository repository;

	public DelegateCommand StartTestingCommand { get; set; }
	public DelegateCommand<TestDataPreview> EditTestCommand { get; set; }
	public DelegateCommand<TestDataPreview> DeleteTestCommand { get; set; }

	public TestsViewModel(TestPreviewProvider testPreviewProvider, IRegionManager regionManager, IDialogService dialogService, TestTypeProvider testTypeProvider, TestingRepository repository) : base()
	{
		this.testPreviewProvider = testPreviewProvider;
		this.regionManager = regionManager;
		this.dialogService = dialogService;
		this.testTypeProvider = testTypeProvider;
		this.repository = repository;
		StartTestingCommand = new DelegateCommand(StartTesting);
		DeleteTestCommand = new DelegateCommand<TestDataPreview>(DeleteTest);
		EditTestCommand = new DelegateCommand<TestDataPreview>(EditTest);
	}

	public void StartTesting() => regionManager.RequestNavigate(RegionNames.ContentRegion, nameof(TestingView));

	private void EditTest(TestDataPreview test)
	{
		var page = "";
		IDialogParameters parameters = new DialogParameters();

		if (test.Type == testTypeProvider.MultipleChoice)
		{
			page = nameof(MultipleChoiceTestCreationView);
			Mneme.Testing.TestCreation.TestMultipleChoices t = repository.GetMultipleChoicesTest(test.Title);
			parameters.Add("test", t);
		}
		else if (test.Type == testTypeProvider.ShortAnswer)
		{
			page = nameof(ShortAnswerTestCreationView);
			Mneme.Testing.TestCreation.TestShortAnswer t = repository.GetShortAnswerTest(test.Title);
			parameters.Add("test", t);
		}
		else
			throw new Exception("Unknown test type or trying to edit cloze deletion");
		dialogService.ShowDialog(page, parameters, result =>
		{
			if (result.Result == ButtonResult.OK)
			{
				var index = AllItems.IndexOf(test);
				AllItems.RemoveAt(index);
				AllItems.Insert(index, result.Parameters.GetValue<TestDataPreview>("test"));
			}
		});
	}

	private void DeleteTest(TestDataPreview test)
	{
		if (test.Type == testTypeProvider.MultipleChoice)
		{
			Mneme.Testing.TestCreation.TestMultipleChoices t = repository.GetMultipleChoicesTest(test.Title);
			repository.RemoveTest(t);
		}
		else if (test.Type == testTypeProvider.ShortAnswer)
		{
			Mneme.Testing.TestCreation.TestShortAnswer t = repository.GetShortAnswerTest(test.Title);
			repository.RemoveTest(t);
		}
		_ = AllItems.Remove(test);
	}

	public async void OnNavigatedTo(NavigationContext navigationContext)
	{
		await Task.Run(() =>
		{
			IReadOnlyList<TestDataPreview> tests = testPreviewProvider.GetAllTests();
			Application.Current.Dispatcher.Invoke(() =>
			{
				if (tests.Count != AllItems.Count)
				{
					AllItems.Clear();
					_ = AllItems.AddRange(tests);
				}
			});
		});
	}

	public bool IsNavigationTarget(NavigationContext navigationContext) => true;

	public void OnNavigatedFrom(NavigationContext navigationContext)
	{

	}

	protected override Func<TestDataPreview, bool> SearchCondition() => x => x.Title.ToLower().Contains(SearchedPhrase.ToLower());
}
