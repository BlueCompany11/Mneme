using System;
using MaterialDesignThemes.Wpf;
using Mneme.Core.Interfaces;
using Mneme.Integrations.Contracts;
using Mneme.Model.Sources;
using Prism.Commands;
using Prism.Mvvm;

namespace Mneme.Desktop.ViewModels
{
	public class MainWindowViewModel : BindableBase
	{
		public ISnackbarMessageQueue SnackbarMessageQueue { get; }

		public MainWindowViewModel( ISnackbarMessageQueue snackbarMessageQueue)
		{
			SnackbarMessageQueue = snackbarMessageQueue;
		}
	}
}
