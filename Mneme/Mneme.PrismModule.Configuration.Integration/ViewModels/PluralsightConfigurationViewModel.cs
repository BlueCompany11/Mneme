using Microsoft.Win32;
using Mneme.Integrations.Pluralsight;
using Prism.Commands;
using Prism.Mvvm;

namespace Mneme.PrismModule.Configuration.Integration.ViewModels
{
	public class PluralsightConfigurationViewModel : BindableBase
	{
		public DelegateCommand LoadFileCommand { get; private set; }
		private string filePath;
		private readonly PluralsightConfigProvider pluralsightConfigProvider;

		public string FilePath
		{
			get => filePath;
			set => SetProperty(ref filePath, value);
		}
		public PluralsightConfigurationViewModel(PluralsightConfigProvider pluralsightConfigProvider) //TODO delete pl reference
		{
			LoadFileCommand = new DelegateCommand(Load);
			this.pluralsightConfigProvider = pluralsightConfigProvider;
			FilePath = pluralsightConfigProvider.Config.FilePath;
		}

		private void Load()
		{
			var openFileDialog = new OpenFileDialog
			{
				Filter = "CSV Files (*.csv)|*.csv"
			};
			if (openFileDialog.ShowDialog() == true)
				FilePath = openFileDialog.FileName;
			pluralsightConfigProvider.UpdatePath(FilePath);
		}
	}
}
