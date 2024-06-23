using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Mneme.PrismModule.Testing.ViewModels.TestCreation;

namespace Mneme.PrismModule.Testing.Views.TestCreation
{
	/// <summary>
	/// Interaction logic for ClozeDeletionTestCreationView.xaml
	/// </summary>
	public partial class ClozeDeletionTestCreationView : UserControl
	{
		public ClozeDeletionTestCreationView()
		{
			InitializeComponent();
			DataContextChanged += ClozeDeletionTestCreationView_DataContextChanged;
		}

		private void ClozeDeletionTestCreationView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			var dataContext = DataContext as ClozeDeletionTestCreationViewModel;
			dataContext.ClearTextFromUi += DataContext_ClearTextFromUi;
			dataContext.AddText += DataContext_AddText;
		}

		private void DataContext_AddText()
		{
			var dataContext = DataContext as ClozeDeletionTestCreationViewModel;
			TextParagraph.Inlines.Add(new Run(dataContext.Text));
		}

		private void DataContext_ClearTextFromUi()
		{
			TextParagraph.Inlines.Clear();
		}

		private void btnGetSelectedText_Click(object sender, RoutedEventArgs e)
		{
			var dataContext = DataContext as ClozeDeletionTestCreationViewModel;
			var positions = Count();

			dataContext.MarkClozeDeletion(positions.Item1, positions.Item2);
			//var previousText = dataContext.Text;
			var range = new TextRange(textRichTextBox.Selection.Start, textRichTextBox.Selection.End);
			range.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Yellow);
			//range.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Green);
			//dataContext.Text = previousText;
		}

		private Tuple<int, int> Count()
		{
			var docStart = textRichTextBox.Document.ContentStart;

			var selectionStart = textRichTextBox.Selection.Start;
			var selectionEnd = textRichTextBox.Selection.End;

			//these will give you the positions needed to apply highlighting
			_ = docStart.GetOffsetToPosition(selectionStart);
			_ = docStart.GetOffsetToPosition(selectionEnd);

			//these values will give you the absolute character positions relative to the very beginning of the text.
			var start = new TextRange(docStart, selectionStart);
			var end = new TextRange(docStart, selectionEnd);
			int indexStart_abs = start.Text.Length;
			int indexEnd_abs = end.Text.Length;
			return new Tuple<int, int>(indexStart_abs, indexEnd_abs);
		}
	}
}
