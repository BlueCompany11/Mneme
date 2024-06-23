using Mneme.Model.TestCreation;
using Mneme.Testing.TestCreation;
using Prism.Mvvm;
using Prism.Regions;

namespace Mneme.PrismModule.Testing.ViewModels.UsersTests
{
	public class ShortAnswerTestViewModel : BindableBase, INavigationAware
	{
		private readonly TestImportanceMapper testImportanceMapper;
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
		public int AnswerDifficulty { get; set; }
		public string Hint { get; set; }

		public string Importance { get; set; }
		public string LastDifficulty { get; set; }
		private TestShortAnswer Test { get; set; }
		public ShortAnswerTestViewModel(TestImportanceMapper testImportanceMapper)
		{
			this.testImportanceMapper = testImportanceMapper;
		}

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			Test = navigationContext.Parameters.GetValue<IUserTest>("test") as TestShortAnswer;
			Question = Test.Question;
			Answer = Test.Answer;
			Hint = Test.Hint;
			Importance = testImportanceMapper.Map(Test.Importance);
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
