using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Mneme.Model.TestCreation;
using Mneme.Testing.UsersTests;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Mneme.PrismModule.Testing.ViewModels.UsersTests
{
	public class TestingViewModel : BindableBase, INavigationAware
	{
		private readonly TestPreviewProvider testPreviewProvider;
		private readonly IRegionManager regionManager;
		private CancellationTokenSource cts;

		private Queue<IUserTest> UserTests { get; set; }
		public DelegateCommand WrongAnswerCommand { get; set; }
		public DelegateCommand CorrectAnswerCommand { get; set; }
		public DelegateCommand ShowAnswerCommand { get; set; }
		public DelegateCommand ShowHintCommand { get; set; }

		private bool finishedTesting;
		public bool FinishedTesting
		{
			get => finishedTesting;
			set => SetProperty(ref finishedTesting, value);
		}
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

		private bool displayAnswer;
		public bool DisplayAnswer

		{
			get => displayAnswer;
			set => SetProperty(ref displayAnswer, value);
		}
		private bool allowToDisplayAnswer;
		public bool AllowToDisplayAnswer

		{
			get => allowToDisplayAnswer;
			set => SetProperty(ref allowToDisplayAnswer, value);
		}

		private bool allowToValidateAnswer;
		public bool AllowToValidateAnswer

		{
			get => allowToValidateAnswer;
			set => SetProperty(ref allowToValidateAnswer, value);
		}
		private bool showHint;
		public bool ShowHint

		{
			get => showHint;
			set => SetProperty(ref showHint, value);
		}
		
		private IUserTest CurrentTest { get; set; }
		public TestingViewModel(TestPreviewProvider testPreviewProvider, IRegionManager regionManager)
		{
			this.testPreviewProvider = testPreviewProvider;
			this.regionManager = regionManager;
			CorrectAnswerCommand = new DelegateCommand(NextTest);
			WrongAnswerCommand = new DelegateCommand(NextTest); //TODO
			ShowAnswerCommand = new DelegateCommand(ShowAnswer);
			ShowHintCommand = new DelegateCommand(DisplayHint, () => !string.IsNullOrEmpty(Hint)).ObservesProperty(() => Hint);
		}

		private void DisplayHint()
		{
			ShowHint = true;
		}

		private void ShowAnswer()
		{
			QuestionStage(false);
		}

		private void NextTest()
		{
			if (CheckIfTestsAreFinished())
				return;
			CurrentTest = UserTests.Dequeue();
			
			if (CurrentTest is TestShortAnswer tsa)
			{
				MapProperties(tsa);
			}
			else if (CurrentTest is TestMultipleChoices tmc)
			{
				MapProperties(tmc);
			}
		}

		private void MapProperties(TestMultipleChoices tmc)
		{
			Question = tmc.Question;
			var shuffledAnswers = ShuffleAnswers(tmc.Answers);
			Hint = GenerateHint(shuffledAnswers);
			var correctAnswer = "";
			for(int i=0 ; i < shuffledAnswers.Count ; i++)
			{
				if(shuffledAnswers[i].IsCorrect)
					correctAnswer = $"{(char)('A' + i)}: {shuffledAnswers[i].Answer},";
			}
			correctAnswer = correctAnswer.TrimEnd(',');
			Answer = correctAnswer;
		}
		private void MapProperties(TestShortAnswer tsa)
		{
			Question = tsa.Question;
			Answer = tsa.Answer;
			Hint = tsa.Hint;
		}

		private bool CheckIfTestsAreFinished()
		{
			if (UserTests.TryPeek(out var test))
			{
				QuestionStage(true);
				return false;
			}
			FinishedTesting = true;
			AllowToDisplayAnswer = false;
			AllowToValidateAnswer = false;
			DisplayAnswer = false;
			ShowHint = false;
			return true;
		}
		private List<TestMultipleChoice> ShuffleAnswers(List<TestMultipleChoice> answers)
		{
			var random = new Random();
			return answers.OrderBy(x => random.Next()).Where(x => !string.IsNullOrEmpty(x.Answer)).ToList();
		}
		private string GenerateHint(List<TestMultipleChoice> answers)
		{
			var hint = "";
			for (int i = 0 ; i < answers.Count ; i++)
			{
				hint += $"{(char)('A' + i)}: {answers[i].Answer} ";
			}
			return hint.Trim();
		}
		private void QuestionStage(bool isQuestionStage)
		{
			AllowToDisplayAnswer = isQuestionStage;
			AllowToValidateAnswer = !isQuestionStage;
			DisplayAnswer = !isQuestionStage;
			ShowHint = !isQuestionStage;
		}

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return true;
		}

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{
			if (UserTests.Count > 0)
			{
				cts?.Cancel();
			}
		}

		public async void OnNavigatedTo(NavigationContext navigationContext)
		{
			using (cts = new CancellationTokenSource())
			{
				await Task.Run(() =>
				{
					UserTests = testPreviewProvider.GetTestsForToday();
					Application.Current.Dispatcher.Invoke(() =>
					{
						NextTest();
					});
				}, cts.Token);
			}
			cts = null;
		}
	}
}
