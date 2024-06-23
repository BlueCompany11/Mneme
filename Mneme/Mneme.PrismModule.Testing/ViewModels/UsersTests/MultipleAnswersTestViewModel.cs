using System.Collections.ObjectModel;
using System.Linq;
using Mneme.Model.TestCreation;
using Mneme.Testing.TestCreation;
using Prism.Mvvm;
using Prism.Regions;

namespace Mneme.PrismModule.Testing.ViewModels.UsersTests
{
	public class MultipleAnswersTestViewModel : BindableBase, INavigationAware
	{
		private readonly TestImportanceMapper testImportanceMapper;
		private string question;
		public string Question
		{
			get => question;
			set => SetProperty(ref question, value);
		}
		private ObservableCollection<string> answers;
		public ObservableCollection<string> Answers
		{
			get => answers;
			set => SetProperty(ref answers, value);
		}
		public int AnswerDifficulty { get; set; }

		public string Importance { get; set; }
		public string LastDifficulty { get; set; }
		private TestMultipleChoices Test { get; set; }
		private string correctAnswer;
		public string CorrectAnswer
		{
			get => correctAnswer;
			set => SetProperty(ref correctAnswer, value);
		}
		public MultipleAnswersTestViewModel(TestImportanceMapper testImportanceMapper)
		{
			this.testImportanceMapper = testImportanceMapper;
		}
		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			Test = navigationContext.Parameters.GetValue<IUserTest>("test") as TestMultipleChoices;
			Question = Test.Question;
			Answers = [];
			for (int i = 0 ; i < Test.Answers.Count ; i++)
			{
				Answers.Add(Test.Answers[i].Answer);
			}
			Importance = testImportanceMapper.Map(Test.Importance);
			var correctAnswers = Test.Answers.Where(x => x.IsCorrect == true).ToList();
			if (Test.Answers[0].IsCorrect)
			{
				CorrectAnswer = "A";
			}
			if (Test.Answers[1].IsCorrect)
			{
				CorrectAnswer += ", B";
			}
			if (Test.Answers[2].IsCorrect)
			{
				CorrectAnswer += ", C";
			}
			if (Test.Answers[3].IsCorrect)
			{
				CorrectAnswer += ", D";
			}
			if (Test.Answers[4].IsCorrect)
			{
				CorrectAnswer += ", E";
			}
			if (Test.Answers[5].IsCorrect)
			{
				CorrectAnswer += ", F";
			}
		}

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return true;
		}

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{

		}
	}
}
