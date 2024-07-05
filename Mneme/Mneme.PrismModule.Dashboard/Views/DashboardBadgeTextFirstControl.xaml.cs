using System.Windows;
using System.Windows.Controls;

namespace Mneme.PrismModule.Dashboard.Views
{
	/// <summary>
	/// Interaction logic for DashboardBadgeTextFirstControl.xaml
	/// </summary>
	public partial class DashboardBadgeTextFirstControl : UserControl
	{
		public DashboardBadgeTextFirstControl()
		{
			InitializeComponent();
		}
		public string Header
		{
			get => (string)GetValue(HeaderProperty);
			set => SetValue(HeaderProperty, value);
		}

		public static readonly DependencyProperty HeaderProperty =
			DependencyProperty.Register("Header", typeof(string), typeof(DashboardBadgeTextFirstControl), new PropertyMetadata(string.Empty));

		public string Description
		{
			get => (string)GetValue(DescriptionProperty);
			set => SetValue(DescriptionProperty, value);
		}

		public static readonly DependencyProperty DescriptionProperty =
			DependencyProperty.Register("Description", typeof(string), typeof(DashboardBadgeTextFirstControl), new PropertyMetadata(string.Empty));
	}
}
