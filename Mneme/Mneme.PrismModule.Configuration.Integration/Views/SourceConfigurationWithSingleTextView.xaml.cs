using System.Windows.Controls;

namespace Mneme.PrismModule.Configuration.Integration.Views;

/// <summary>
/// Interaction logic for SourceConfigurationWithSignleTextView.xaml
/// </summary>
public partial class SourceConfigurationWithSignleTextView : UserControl
{
	public string SourceName { get; set; }
	public string Text1 { get; set; }
	public string Text2 { get; set; }
	public SourceConfigurationWithSignleTextView() => InitializeComponent();

}
