using Mneme.Views.Base;
using Prism.Navigation.Regions;
using System.Windows;
using System.Windows.Controls;

namespace Mneme.PrismModule.Testing.Views.TestCreation;

/// <summary>
/// Interaction logic for TestCreationView.xaml
/// </summary>
public partial class TestCreationView : UserControl
{
	private readonly IRegionManager regionManager;

	public TestCreationView(IRegionManager regionManager)
	{
		InitializeComponent();
		this.regionManager = regionManager;
		Loaded += TestCreationView_Loaded;
	}

	private void TestCreationView_Loaded(object sender, RoutedEventArgs e)
	{
		RegionManager.SetRegionManager(NotePreviewContentControl, regionManager);
		RegionManager.SetRegionName(NotePreviewContentControl, RegionNames.NotePreviewRegion);
	}
}
