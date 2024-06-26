using Mneme.Model.Preelaborations;
using Mneme.Views.Base;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Mneme.PrismModule.Integrations.Base
{
	public abstract class AbstractNotePreviewViewModel : BindableBase, INavigationAware
	{
		protected readonly IRegionManager regionManager;
		private bool showCreateTestButton;
		public bool ShowCreateTestButton
		{
			get => showCreateTestButton;
			set => SetProperty(ref showCreateTestButton, value);
		}

		public AbstractNotePreviewViewModel(IRegionManager regionManager)
		{
			this.regionManager = regionManager;
			CreateTestCommand = new DelegateCommand(Navigate);
			ShowCreateTestButton = true;
		}
		public DelegateCommand CreateTestCommand { get; set; }
		protected abstract Preelaboration Preelaboration { get; set; }
		protected abstract void LoadNote();
		protected void CheckIfShouldDisplayCreateTestButton(NavigationContext navigationContext)
		{
			if (navigationContext.Parameters.TryGetValue<bool>("showCreateTestButton", out var showCreateTestButton))
				ShowCreateTestButton = showCreateTestButton;
		}
		private void Navigate()
		{
			var param = new NavigationParameters
			{
				{"pre", Preelaboration },
				{"showCreateTestButton", false}
			};
			regionManager.RequestNavigate(RegionNames.ContentRegion, "TestCreationView", param); //TODO
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
			CheckIfShouldDisplayCreateTestButton(navigationContext);
			var receivedPreelaboration = navigationContext.Parameters.GetValue<Preelaboration>("pre");
			Preelaboration = receivedPreelaboration;
			LoadNote();
		}
	}
}
