using MaterialDesignThemes.Wpf;
using Prism.Mvvm;

namespace Mneme.Desktop.ViewModels;

public class MainWindowViewModel : BindableBase
{
	public ISnackbarMessageQueue SnackbarMessageQueue { get; }

	public MainWindowViewModel(ISnackbarMessageQueue snackbarMessageQueue) => SnackbarMessageQueue = snackbarMessageQueue;
}
