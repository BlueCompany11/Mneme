using Mneme.Model.TestCreation;
using Mneme.Testing.UsersTests;
using Prism.Mvvm;
using Prism.Regions;

namespace Mneme.PrismModule.Testing.ViewModels.UsersTests
{
	public class ClozeDeletionTestViewModel : BindableBase, INavigationAware
	{
		private TestClozeDeletion Test { get; set; }
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

		private readonly ClozeTestTextHelper clozeTestTextHelper;

		public ClozeDeletionTestViewModel(ClozeTestTextHelper clozeTestTextHelper)
		{
			this.clozeTestTextHelper = clozeTestTextHelper;
		}
		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return true;
		}

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{

		}

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			Test = navigationContext.Parameters.GetValue<IUserTest>("test") as TestClozeDeletion;
			Question = clozeTestTextHelper.GetTextAsQuestion(Test);
			Answer = string.Join('\n', clozeTestTextHelper.GetTextAsAnser(Test));
		}
	}
}
