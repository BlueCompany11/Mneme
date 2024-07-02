using System;
using System.Collections.Generic;
using MaterialDesignThemes.Wpf;
using Mneme.Model.Interfaces;
using Mneme.Model.Notes;
using Mneme.Model.TestCreation;
using Mneme.Testing.Contracts;
using Mneme.Testing.Database;
using Mneme.Testing.TestCreation;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Mneme.PrismModule.Testing.ViewModels.TestCreation
{
	public class MultipleChoiceTestCreationViewModel : BindableBase, INavigationAware
	{
		private readonly int amountOfAnswers = 6;
		private string question;
		public string Question
		{
			get => question;
			set => SetProperty(ref question, value);
		}

		private List<bool> check;
		public List<bool> Checks
		{
			get => check;
			set => SetProperty(ref check, value);
		}
		private List<string> text;


		public List<string> Texts
		{
			get => text;
			set => SetProperty(ref text, value);
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
		private readonly INoteTestVisitor multipleChoiceNoteTestVisitor;
		private readonly TestImportanceMapper testImportanceMapper;
		private readonly ISnackbarMessageQueue snackbarMessageQueue;
		private readonly TestingRepository repository;

		public MultipleChoiceTestCreationViewModel(MultipleChoiceNoteTestVisitor multipleChoiceNoteTestVisitor, TestImportanceMapper testImportanceMapper, ISnackbarMessageQueue snackbarMessageQueue, TestingRepository repository)
		{
			ImportanceOptions = testImportanceMapper.ImportanceOptions;
			SelectedImportanceOption = ImportanceOptions[0];
			this.multipleChoiceNoteTestVisitor = multipleChoiceNoteTestVisitor;
			this.testImportanceMapper = testImportanceMapper;
			this.snackbarMessageQueue = snackbarMessageQueue;
			this.repository = repository;
			Texts = new List<string>(amountOfAnswers);
			Checks = new List<bool>(amountOfAnswers);
			for (int i = 0 ; i < amountOfAnswers ; i++)
			{
				Texts.Add(string.Empty);
				Checks.Add(false);
			}
			CreateTestCommand = new DelegateCommand(CreateTest);
		}

		private Note Note { get; set; }
		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			Note = navigationContext.Parameters.GetValue<Note>("note");
			var data = Note.Accept(multipleChoiceNoteTestVisitor) as MultipleChoiceNoteData;
			Question = data.Question;
		}

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return true;
		}

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{

		}
		public DelegateCommand CreateTestCommand { get; set; }

		private void CreateTest()
		{
			var validations = !string.IsNullOrWhiteSpace(Question);
			for (int i = 0 ; i < amountOfAnswers ; i++)
			{
				if(Checks[i])
					validations = validations && !string.IsNullOrWhiteSpace(Texts[i]);
			}
			if (!validations)
			{
				snackbarMessageQueue.Enqueue("Question and correct answers are required");
				return;
			}
			int importance = testImportanceMapper.Map(SelectedImportanceOption);
			var answers = new List<TestMultipleChoice>();
			for (int i = 0 ; i < amountOfAnswers ; i++)
			{
				answers.Add(new TestMultipleChoice { Answer = Texts[i], IsCorrect = Checks[i] });
			}
			var test = new TestMultipleChoices { Question = Question, Answers = answers, Importance = importance, Created = DateTime.Now, NoteId = Note.IntegrationId };
			repository.CreateTest(test);
			snackbarMessageQueue.Enqueue("Test created");
		}
	}
}
