using Mneme.Model;
using Mneme.Testing.RepetitionAlgorithm;
using Mneme.Testing.UsersTests;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Mneme.PrismModule.Testing.ViewModels.UsersTests;

public class TestingViewModel : BindableBase, INavigationAware
{
	private readonly TestPreviewProvider testPreviewProvider;
	private readonly IRegionManager regionManager;
	private readonly SpaceRepetition speceRepetition;
	private CancellationTokenSource cts;
	private Test? currentTest;

	private Queue<Test> UserTests { get; set; }
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

	public TestingViewModel(TestPreviewProvider testPreviewProvider, IRegionManager regionManager, SpaceRepetition speceRepetition)
	{
		this.testPreviewProvider = testPreviewProvider;
		this.regionManager = regionManager;
		this.speceRepetition = speceRepetition;
		CorrectAnswerCommand = new DelegateCommand(CorrectAnswer);
		WrongAnswerCommand = new DelegateCommand(IncorrectAnswer);
		ShowAnswerCommand = new DelegateCommand(ShowAnswer);
		ShowHintCommand = new DelegateCommand(DisplayHint, () => !string.IsNullOrEmpty(Hint)).ObservesProperty(() => Hint);
	}

	private void DisplayHint() => ShowHint = true;

	private void ShowAnswer() => QuestionStage(false);
	private void CorrectAnswer() => NextTestPlusCheckUserAnswer(true);
	private void IncorrectAnswer() => NextTestPlusCheckUserAnswer(false);
	private void NextTest()
	{
		if (CheckIfTestsAreFinished())
			return;

		Question = currentTest.Question;
		Answer = currentTest.GetAnswer();
		Hint = currentTest.GetHint();
	}
	private void NextTestPlusCheckUserAnswer(bool success)
	{
		speceRepetition.MakeTest(currentTest, success);
		NextTest();
	}

	private bool CheckIfTestsAreFinished()
	{
		if (UserTests.TryPeek(out Test test))
		{
			currentTest = UserTests.Dequeue();
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


	private void QuestionStage(bool isQuestionStage)
	{
		AllowToDisplayAnswer = isQuestionStage;
		AllowToValidateAnswer = !isQuestionStage;
		DisplayAnswer = !isQuestionStage;
		ShowHint = !isQuestionStage;
		FinishedTesting = false;
	}

	public bool IsNavigationTarget(NavigationContext navigationContext) => true;

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
