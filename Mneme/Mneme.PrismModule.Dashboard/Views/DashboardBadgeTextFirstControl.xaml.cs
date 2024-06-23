using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
