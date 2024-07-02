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
	public class ShortAnswerTestCreationViewModel : BindableBase, INavigationAware
	{
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
		private readonly INoteTestVisitor shortAnswerNoteTestVisitor;
		private readonly TestImportanceMapper testImportanceMapper;
		private readonly ISnackbarMessageQueue snackbarMessageQueue;
		private readonly TestingRepository repository;

		private Note Note { get; set; }
		public ShortAnswerTestCreationViewModel(ShortAnswerNoteTestVisitor shortAnswerNoteTestVisitor, TestImportanceMapper testImportanceMapper, ISnackbarMessageQueue snackbarMessageQueue, TestingRepository repository)
		{
			this.shortAnswerNoteTestVisitor = shortAnswerNoteTestVisitor;
			this.testImportanceMapper = testImportanceMapper;
			this.snackbarMessageQueue = snackbarMessageQueue;
			this.repository = repository;
			ImportanceOptions = testImportanceMapper.ImportanceOptions;
			SelectedImportanceOption = ImportanceOptions[0];
			CreateTestCommand = new DelegateCommand(CreateTest);

		}
		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			Note = navigationContext.Parameters.GetValue<Note>("note");
			var data = Note.Accept(shortAnswerNoteTestVisitor) as ShortAnswerNoteData;
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
			var validation = !string.IsNullOrWhiteSpace(Question) && !string.IsNullOrWhiteSpace(Answer);
			if (!validation)
			{
				snackbarMessageQueue.Enqueue("Question and answer cannot be empty");
				return;
			}

			int importance = testImportanceMapper.Map(SelectedImportanceOption);
			var test = new TestShortAnswer { Question = Question, Answer = Answer, Hint = Hint, Importance = importance, Created = DateTime.Now, NoteId = Note.IntegrationId };
			repository.CreateTest(test);
			snackbarMessageQueue.Enqueue("Test created");
		}
	}
}
