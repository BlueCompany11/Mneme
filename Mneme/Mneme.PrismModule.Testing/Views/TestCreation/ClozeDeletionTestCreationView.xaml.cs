using Mneme.PrismModule.Testing.ViewModels.TestCreation;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Mneme.PrismModule.Testing.Views.TestCreation;

/// <summary>
/// Interaction logic for ClozeDeletionTestCreationView.xaml
/// </summary>
public partial class ClozeDeletionTestCreationView : UserControl
{
	public ClozeDeletionTestCreationView() => InitializeComponent();

	private void btnGetSelectedText_Click(object sender, RoutedEventArgs e)
	{
		var dataContext = DataContext as ClozeDeletionTestCreationViewModel;
		var positions = Count();

		dataContext.MarkClozeDeletion(positions.Item1, positions.Item2);
	}

	private Tuple<int, int> Count()
	{
		var selectionStart = textRichTextBox.SelectionStart;
		var selectionEnd = textRichTextBox.SelectionStart + textRichTextBox.SelectionLength;

		return new Tuple<int, int>(selectionStart, selectionEnd);
	}
}
