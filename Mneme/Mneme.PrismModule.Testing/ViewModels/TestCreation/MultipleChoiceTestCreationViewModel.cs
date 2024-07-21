using System;
using System.Collections.Generic;
using System.Linq;
using ImTools;
using MaterialDesignThemes.Wpf;
using Mneme.Model;
using Mneme.Testing.Contracts;
using Mneme.Testing.TestCreation;
using Mneme.Testing.UsersTests;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace Mneme.PrismModule.Testing.ViewModels.TestCreation
{
	public class MultipleChoiceTestCreationViewModel : BindableBase, INavigationAware, IDialogAware
	{
		bool editMode;
		string oldQuestion;
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

		private readonly TestImportanceMapper testImportanceMapper;
		private readonly ISnackbarMessageQueue snackbarMessageQueue;
		private readonly TestingRepository repository;
		private readonly TestTypeProvider testTypeProvider;

		public event Action<IDialogResult> RequestClose;

		public MultipleChoiceTestCreationViewModel(TestImportanceMapper testImportanceMapper, ISnackbarMessageQueue snackbarMessageQueue, TestingRepository repository, TestTypeProvider testTypeProvider)
		{
			ImportanceOptions = testImportanceMapper.ImportanceOptions;
			SelectedImportanceOption = ImportanceOptions[0];
			this.testImportanceMapper = testImportanceMapper;
			this.snackbarMessageQueue = snackbarMessageQueue;
			this.repository = repository;
			this.testTypeProvider = testTypeProvider;
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
			Question = Note.Title;
			editMode = false;
		}

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return true;
		}

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{

		}
		public DelegateCommand CreateTestCommand { get; set; }

		public string Title => "Edit";

		private void CreateTest()
		{
			if (!Validate())
				return;
			if (editMode)
			{
				var test = repository.GetMultipleChoicesTest(oldQuestion);
				test.Question = Question;
				test.Answers.Clear();
				for (int i = 0 ; i < amountOfAnswers ; i++)
				{
					test.Answers.Add(new TestMultipleChoice { Answer = Texts[i], IsCorrect = Checks[i] });
				}
				test.Importance = testImportanceMapper.Map(SelectedImportanceOption);
				repository.EditTest(test);
				var param = new DialogParameters
				{
					{ "test", new TestDataPreview { Title = test.Question, CreationTime = test.Created, Type = testTypeProvider.MultipleChoice }}
				};
				RequestClose?.Invoke(new DialogResult(ButtonResult.OK, param));
				snackbarMessageQueue.Enqueue("Test updated");
			}
			else
			{
				int importance = testImportanceMapper.Map(SelectedImportanceOption);
				var answers = new List<TestMultipleChoice>();
				for (int i = 0 ; i < amountOfAnswers ; i++)
				{
					answers.Add(new TestMultipleChoice { Answer = Texts[i], IsCorrect = Checks[i] });
				}
				var test = new TestMultipleChoices { Question = Question, Answers = answers, Importance = importance, Created = DateTime.Now, NoteId = Note.Id };
				repository.CreateTest(test);
				snackbarMessageQueue.Enqueue("Test created");
			}
		}

		private bool Validate()
		{
			var validations = !string.IsNullOrWhiteSpace(Question) && Checks.Any(x => x == true);

			for (int i = 0 ; i < amountOfAnswers ; i++)
			{
				if (Checks[i])
					validations = validations && !string.IsNullOrWhiteSpace(Texts[i]);
			}
			if (!validations)
			{
				snackbarMessageQueue.Enqueue("Question and correct answers are required");
				return false;
			}
			if (repository.GetMultipleChoicesTest(Question) == null)
				return true;
			else
			{
				snackbarMessageQueue.Enqueue("The question and this test type already exists.");
				return false;
			}
		}

		public bool CanCloseDialog()
		{
			return true;
		}

		public void OnDialogClosed()
		{

		}

		public void OnDialogOpened(IDialogParameters parameters)
		{
			parameters.TryGetValue("test", out TestMultipleChoices test);
			Question = test.Question;
			for (int i = 0 ; i < amountOfAnswers ; i++)
			{
				Texts[i] = test.Answers[i].Answer;
				Checks[i] = test.Answers[i].IsCorrect;
			}
			SelectedImportanceOption = testImportanceMapper.Map(test.Importance);
			editMode = true;
			oldQuestion = Question;
		}
	}
}
