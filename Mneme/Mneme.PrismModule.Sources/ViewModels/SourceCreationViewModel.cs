using System;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using Mneme.Model.Sources;
using Mneme.Sources;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace Mneme.PrismModule.Sources.ViewModels
{
	internal class SourceCreationViewModel : BindableBase, IDialogAware
	{
		private ISnackbarMessageQueue snackbarMessageQueue { get; }
		public DelegateCommand CreateCommand { get; private set; }

		private SourcePreview sourceToEdit;

		private string sourceTitle;
		public string SourceTitle
		{
			get => sourceTitle;
			set => SetProperty(ref sourceTitle, value);
		}

		private string details;
		private readonly MnemeSourceManager manager;

		public string Details
		{
			get => details;
			set => SetProperty(ref details, value);
		}
		public SourceCreationViewModel(ISnackbarMessageQueue snackbarMessageQueue, MnemeSourceManager manager)
		{
			this.snackbarMessageQueue = snackbarMessageQueue;
			this.manager = manager;
			CreateCommand = new DelegateCommand(SaveAndClose, CanCreateSource()).ObservesProperty(() => SourceTitle);
		}
		private Func<bool> CanCreateSource()
		{
			return () => !string.IsNullOrEmpty(SourceTitle);
		}

		private async void SaveAndClose()
		{
			if (sourceToEdit == null)
				await Save();
			else
				await Update();
		}

		private async Task Save()
		{
			var source = await manager.SaveMnemeSource(SourceTitle, Details, default);
			if (source is not null)
			{
				var parameters = new DialogParameters
				{
					{ "source", source }
				};
				snackbarMessageQueue.Enqueue("New source created");
				RequestClose?.Invoke(new DialogResult(ButtonResult.OK, parameters));
			}
			else
				snackbarMessageQueue.Enqueue("Source already exisits");
		}
		private async Task Update()
		{
			sourceToEdit = SourcePreview.CreateFromSource(await manager.UpdateMnemeSource(sourceToEdit.Id, SourceTitle, Details, default));
			var parameters = new DialogParameters
				{
					{ "source", sourceToEdit }
				};
			RequestClose?.Invoke(new DialogResult(ButtonResult.OK, parameters));
			snackbarMessageQueue.Enqueue("Source updated");
		}
		private string title;
		public string Title => title;
		public event Action<IDialogResult> RequestClose;

		public bool CanCloseDialog()
		{
			return true;
		}

		public void OnDialogClosed()
		{

		}

		public void OnDialogOpened(IDialogParameters parameters)
		{
			if (parameters.Count == 0)
			{
				title = "Source creation";
				return;
			}
			sourceToEdit = parameters.GetValue<SourcePreview>("source");
			title = "Source edition";
			SourceTitle = sourceToEdit.Title;
			Details = sourceToEdit.Details;
		}
	}
}
