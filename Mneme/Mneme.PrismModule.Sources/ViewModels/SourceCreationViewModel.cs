using MaterialDesignThemes.Wpf;
using Mneme.Model;
using Mneme.Sources;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;

namespace Mneme.PrismModule.Sources.ViewModels;

internal class SourceCreationViewModel : BindableBase, IDialogAware
{
	private ISnackbarMessageQueue snackbarMessageQueue { get; }
	public DelegateCommand CreateCommand { get; private set; }

	private Source sourceToEdit;

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
	private Func<bool> CanCreateSource() => () => !string.IsNullOrEmpty(SourceTitle);

	private async void SaveAndClose()
	{
		if (sourceToEdit == null)
			await Save();
		else
			await Update();
	}

	private async Task Save()
	{
		Integrations.Mneme.Contract.MnemeSource source = await manager.SaveMnemeSource(SourceTitle, Details, default);
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
		sourceToEdit = await manager.UpdateMnemeSource(sourceToEdit.Id, SourceTitle, Details, default);
		var parameters = new DialogParameters
				{
					{ "source", sourceToEdit }
				};
		RequestClose?.Invoke(new DialogResult(ButtonResult.OK, parameters));
		snackbarMessageQueue.Enqueue("Source updated");
	}

	public string Title { get; private set; }
	public event Action<IDialogResult> RequestClose;

	public bool CanCloseDialog() => true;

	public void OnDialogClosed()
	{

	}

	public void OnDialogOpened(IDialogParameters parameters)
	{
		if (parameters.Count == 0)
		{
			Title = "Source creation";
			return;
		}
		sourceToEdit = parameters.GetValue<Source>("source");
		Title = "Source edition";
		SourceTitle = sourceToEdit.Title;
		Details = sourceToEdit.GetDetails();
	}
}
