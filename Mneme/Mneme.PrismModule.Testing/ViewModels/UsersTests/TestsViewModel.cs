using System.Collections.ObjectModel;
using Mneme.Testing.UsersTests;
using Mneme.Views.Base;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Mneme.PrismModule.Testing.ViewModels.UsersTests
{
	public class TestsViewModel : BindableBase, INavigationAware
	{
		private readonly TestPreviewProvider testPreviewProvider;
		private readonly IRegionManager regionManager;
		private readonly TestTypeProvider testTypeProvider;
		private ObservableCollection<TestDataPreview> tests;

		public DelegateCommand StartTestingCommand { get; set; }
		public ObservableCollection<TestDataPreview> Tests
		{
			get => tests;
			set
			{
				tests = value;
				RaisePropertyChanged(nameof(Tests));
			}
		}
		public TestsViewModel(TestPreviewProvider testPreviewProvider, IRegionManager regionManager, TestTypeProvider testTypeProvider)
		{
			this.testPreviewProvider = testPreviewProvider;
			this.regionManager = regionManager;
			this.testTypeProvider = testTypeProvider;
			StartTestingCommand = new DelegateCommand(StartTesting);
		}


		public void StartTesting()
		{
			regionManager.RequestNavigate(RegionNames.ContentRegion, "TestingView"); //TODO
		}

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			Tests?.Clear();
			Tests = new ObservableCollection<TestDataPreview>(testPreviewProvider.GetTests());
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
