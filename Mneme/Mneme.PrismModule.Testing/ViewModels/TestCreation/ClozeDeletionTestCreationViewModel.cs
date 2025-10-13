using MaterialDesignThemes.Wpf;
using Mneme.Model;
using Mneme.Testing.Contracts;
using Mneme.Testing.TestCreation;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Mneme.PrismModule.Testing.ViewModels.TestCreation;

public class ClozeDeletionTestCreationViewModel : BindableBase, INavigationAware
{
	private string text;
	public string Text
	{
		get => text;
		set => SetProperty(ref text, value);
	}

	public ObservableCollection<string> ClozeDeletions { get; set; }

	private string hint;

	public string Hint
	{
		get => hint;
		set => SetProperty(ref hint, value);
	}
	private List<string> importanceOptions;
	public List<string> ImportanceOptions
	{
		get => importanceOptions;
		set => SetProperty(ref importanceOptions, value);
	}
	private string selectedImportanceOption;
	public string SelectedImportanceOption
	{
		get => selectedImportanceOption;
		set => SetProperty(ref selectedImportanceOption, value);
	}

	private readonly List<TestShortAnswer> tests;
	private Note Note { get; set; }
	public DelegateCommand CreateTestCommand { get; set; }
	private readonly TestImportanceMapper testImportanceMapper;
	private readonly ISnackbarMessageQueue snackbarMessageQueue;
	private readonly TestingRepository repository;

	public ClozeDeletionTestCreationViewModel(TestImportanceMapper testImportanceMapper, ISnackbarMessageQueue snackbarMessageQueue, TestingRepository repository)
	{
		ClozeDeletions = [];
		this.testImportanceMapper = testImportanceMapper;
		this.snackbarMessageQueue = snackbarMessageQueue;
		this.repository = repository;
		ImportanceOptions = testImportanceMapper.ImportanceOptions;
		SelectedImportanceOption = ImportanceOptions[0];
		CreateTestCommand = new DelegateCommand(CreateTest);
		tests = [];
	}

	private void CreateTest()
	{
		var validation = ClozeDeletions.Count > 0;
		if (!validation)
		{
			snackbarMessageQueue.Enqueue("Add cloze deletion first.");
			return;
		}

		_ = testImportanceMapper.Map(SelectedImportanceOption);
		foreach (var test in tests)
		{
			repository.CreateTest(test);
		}
		var message = tests.Count == 1 ? "Test created" : "Tests created";
		snackbarMessageQueue.Enqueue(message);
	}
	public void MarkClozeDeletion(int start, int end)
	{
		var validation = start < end && start >= 0 && end <= Text.Length;
		if (!validation)
		{
			snackbarMessageQueue.Enqueue("Marked text is invalid.");
			return;
		}
		var answer = Text[start..end];
		var question = Text[..start] + " _____ " + Text[end..];
		if (repository.GetShortAnswerTest(question) != null || tests.Where(x => x.Question == question).Count() != 0)
		{
			snackbarMessageQueue.Enqueue("Such test already exisits.");
			return;
		}
		tests.Add(new TestShortAnswer { Question = question, Answer = answer, Importance = testImportanceMapper.Map(SelectedImportanceOption), Created = DateTime.Now, NoteId = Note.Id });
		ClozeDeletions.Add(answer);
		Text = Note.Content;
	}
	public void OnNavigatedTo(NavigationContext navigationContext)
	{
		Note = navigationContext.Parameters.GetValue<Note>("note");
		Text = Note.Content;
		ClozeDeletions.Clear();
	}

	public bool IsNavigationTarget(NavigationContext navigationContext) => true;

	public void OnNavigatedFrom(NavigationContext navigationContext)
	{

	}
}
