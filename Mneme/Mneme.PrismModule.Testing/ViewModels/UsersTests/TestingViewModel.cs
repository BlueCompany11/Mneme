﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Mneme.Model;
using Mneme.Model.TestCreation;
using Mneme.PrismModule.Testing.Views.UsersTests;
using Mneme.Testing.TestCreation;
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

		public void NextTest()
		{
			var test = CurrentTest;

			bool isNextTest = UserTests.TryPeek(out test);
			if (!isNextTest)
			{
				FinishedTesting = true;
				AllowToDisplayAnswer = false;
				AllowToValidateAnswer = false;
				DisplayAnswer = false;
				ShowHint = false;
				return;
			}
			CurrentTest = test;
			CurrentTest.Tested(true);
			QuestionStage(true);
			var param = new NavigationParameters()
						{
								{ "test", UserTests.Dequeue() }
						};
			if (CurrentTest is TestShortAnswer tsa)
			{
				Question = tsa.Question;
				Answer = tsa.Answer;
				Hint = tsa.Hint;
			}
			else if (CurrentTest is TestMultipleChoices tmc)
			{
				Question = tmc.Question;


				List<string> Answers = new List<string>();
				for (int i = 0 ; i < tmc.Answers.Count ; i++)
				{
					Answers.Add(tmc.Answers[i].Answer);
				}
				Hint = GenerateHint(tmc.Answers.Where(x=>!string.IsNullOrEmpty(x.Answer)));
				var correctAnswers = tmc.Answers.Where(x => x.IsCorrect == true).ToList();
				var correctAnswer = "";
				if (tmc.Answers[0].IsCorrect)
				{
					correctAnswer = "A";
				}
				if (tmc.Answers[1].IsCorrect)
				{
					correctAnswer += "B";
				}
				if (tmc.Answers[2].IsCorrect)
				{
					correctAnswer += "C";
				}
				if (tmc.Answers[3].IsCorrect)
				{
					correctAnswer += "D";
				}
				if (tmc.Answers[4].IsCorrect)
				{
					correctAnswer += "E";
				}
				if (tmc.Answers[5].IsCorrect)
				{
					correctAnswer += "F";
				}
				Answer = string.Join(", ", correctAnswer.ToCharArray());
			}
		}

		private string GenerateHint(IEnumerable<TestMultipleChoice> answers)
		{
			var random = new Random();
			var shuffledAnswers = answers.OrderBy(x => random.Next()).ToList();
			var hint = "";
			for (int i = 0 ; i < shuffledAnswers.Count ; i++)
			{
				hint += $"{(char)('A' + i)}: {shuffledAnswers[i].Answer} ";
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
