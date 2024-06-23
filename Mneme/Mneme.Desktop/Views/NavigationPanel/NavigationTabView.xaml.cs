using System.Windows;
using System.Windows.Controls;

namespace Mneme.Desktop.Views.NavigationPanel
{
	/// <summary>
	/// Interaction logic for NavigationTabContentControl.xaml
	/// </summary>
	public partial class NavigationTabView : UserControl
	{
		public NavigationTabView()
		{
			InitializeComponent();
		}
		public string SourcePath
		{
			get => (string)GetValue(SourcePathProperty);
			set => SetValue(SourcePathProperty, value);
		}

		public static readonly DependencyProperty SourcePathProperty =
			DependencyProperty.Register("SourcePath", typeof(string), typeof(NavigationTabView), new PropertyMetadata(string.Empty));

		public string Text
		{
			get => (string)GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}

		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register("Text", typeof(string), typeof(NavigationTabView), new PropertyMetadata(string.Empty));

		public string NavigationTarget
		{
			get => (string)GetValue(NavigationTargetProperty);
			set => SetValue(NavigationTargetProperty, value);
		}

		public static readonly DependencyProperty NavigationTargetProperty =
			DependencyProperty.Register("NavigationTarget", typeof(string), typeof(NavigationTabView), new PropertyMetadata(string.Empty));

		public static RoutedEvent ItemClickedEvent = EventManager.RegisterRoutedEvent("ItemClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NavigationTabView));

		public event RoutedEventHandler ItemClicked
		{
			add => AddHandler(ItemClickedEvent, value);
			remove => RemoveHandler(ItemClickedEvent, value);
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			RaiseEvent(new RoutedEventArgs(ItemClickedEvent, this));
		}
	}
}
