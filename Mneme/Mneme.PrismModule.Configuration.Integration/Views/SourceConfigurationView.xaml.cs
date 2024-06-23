using System.Windows.Controls;

namespace Mneme.PrismModule.Configuration.Integration.Views
{
	/// <summary>
	/// Interaction logic for SourceConfigurationView.xaml
	/// </summary>
	public partial class SourceConfigurationView : UserControl
	{
		public string SourceName { get; set; }
		public string Format1 { get; set; }
		public string Format2 { get; set; }
		public string Format3 { get; set; }
		public string Format4 { get; set; }
		public bool IsButtonEnabled { get; set; } = false;
		public SourceConfigurationView()
		{
			InitializeComponent();
			DataContext = this;
		}
	}
}
