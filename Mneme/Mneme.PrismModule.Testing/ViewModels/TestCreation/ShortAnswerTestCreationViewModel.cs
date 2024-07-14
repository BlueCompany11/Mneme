using System;
using System.Collections.Generic;
using MaterialDesignThemes.Wpf;
using Mneme.Model;
using Mneme.Model.TestCreation;
using Mneme.Testing.Contracts;
using Mneme.Testing.TestCreation;
using Mneme.Testing.UsersTests;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace Mneme.PrismModule.Testing.ViewModels.TestCreation
{
	public class ShortAnswerTestCreationViewModel : BindableBase, INavigationAware, IDialogAware
	{
		bool editMode;
		private string question;
		public string Question
		{
			get => question;
			set => SetProperty(ref question, value);
		}

		private string answer;
		public string Answer
		{
			get => answer;
			set => SetProperty(ref answer, value);
		}

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

		private readonly TestImportanceMapper testImportanceMapper;
		private readonly ISnackbarMessageQueue snackbarMessageQueue;
		private readonly TestingRepository repository;
		private readonly TestTypeProvider testTypeProvider;
		private string oldQuestion;

		public event Action<IDialogResult> RequestClose;

		private Note Note { get; set; }
		public ShortAnswerTestCreationViewModel(TestImportanceMapper testImportanceMapper, ISnackbarMessageQueue snackbarMessageQueue, TestingRepository repository, TestTypeProvider testTypeProvider)
		{
			this.testImportanceMapper = testImportanceMapper;
			this.snackbarMessageQueue = snackbarMessageQueue;
			this.repository = repository;
			this.testTypeProvider = testTypeProvider;
			ImportanceOptions = testImportanceMapper.ImportanceOptions;
			SelectedImportanceOption = ImportanceOptions[0];
			CreateTestCommand = new DelegateCommand(CreateTest);

		}
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
			int importance = testImportanceMapper.Map(SelectedImportanceOption);
			if (editMode)
			{
				var test = repository.GetShortAnswerTest(oldQuestion);
				test.Question = Question;
				test.Answer = Answer;
				test.Hint = Hint;
				test.Importance = importance;
				repository.EditTest(test);
				snackbarMessageQueue.Enqueue("Test updated");
				var param = new DialogParameters
				{
					{ "test", new TestDataPreview { Title = test.Question, CreationTime = test.Created, Type = testTypeProvider.ShortAnswer }}
				};
				RequestClose?.Invoke(new DialogResult(ButtonResult.OK, param));
			}
			else
			{
				var test = new TestShortAnswer { Question = Question, Answer = Answer, Hint = Hint, Importance = importance, Created = DateTime.Now, NoteId = Note.Id };
				repository.CreateTest(test);
				snackbarMessageQueue.Enqueue("Test created");
			}
		}

		private bool Validate()
		{
			var validation = !string.IsNullOrWhiteSpace(Question) && !string.IsNullOrWhiteSpace(Answer);
			if (!validation)
			{
				snackbarMessageQueue.Enqueue("Question and answer cannot be empty");
				return false;
			}
			return true;
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
			parameters.TryGetValue("test", out TestShortAnswer test);
			Question = test.Question;
			Answer = test.Answer;
			Hint = test.Hint;
			SelectedImportanceOption = testImportanceMapper.Map(test.Importance);
			oldQuestion = Question;
			editMode = true;
		}
	}
}
