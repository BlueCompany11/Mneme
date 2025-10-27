using MaterialDesignThemes.Wpf;
using Mneme.Model;
using Mneme.Sources;
using Prism.Commands;
using Prism.Dialogs;
using Prism.Mvvm;
using System;
using System.Threading.Tasks;

namespace Mneme.PrismModule.Sources.ViewModels;

internal class SourceCreationViewModel : BindableBase, IDialogAware
{
	private ISnackbarMessageQueue snackbarMessageQueue { get; }
	public DelegateCommand CreateCommand { get; private set; }

	private ISource sourceToEdit;

	private string sourceTitle;
	public string SourceTitle
	{
		get => sourceTitle;
		set => SetProperty(ref sourceTitle, value);
	}

	private string details;
	private readonly MnemeSourceProxy proxy;

	public string Details
	{
		get => details;
		set => SetProperty(ref details, value);
	}
	public SourceCreationViewModel(ISnackbarMessageQueue snackbarMessageQueue, MnemeSourceProxy proxy)
	{
		this.snackbarMessageQueue = snackbarMessageQueue;
		this.proxy = proxy;
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
		var source = await proxy.SaveMnemeSource(SourceTitle, Details, default);
		if (source is not null)
		{
			var parameters = new DialogParameters
				{
					{ "source", source }
				};
			snackbarMessageQueue.Enqueue("New source created");
			var dialogResult = new DialogResult(ButtonResult.OK);
			dialogResult.Parameters = parameters;
			RequestClose?.Invoke(dialogResult);
		} else
			snackbarMessageQueue.Enqueue("Source already exisits");
	}
	private async Task Update()
	{
		sourceToEdit = await proxy.UpdateMnemeSource(sourceToEdit.Id, SourceTitle, Details, default);
		var parameters = new DialogParameters
				{
					{ "source", sourceToEdit }
				};
		var dialogResult = new DialogResult(ButtonResult.OK);
		dialogResult.Parameters = parameters;
		RequestClose?.Invoke(dialogResult);
		snackbarMessageQueue.Enqueue("Source updated");
	}

	public string Title { get; private set; }

	DialogCloseListener IDialogAware.RequestClose => throw new NotImplementedException();

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
		sourceToEdit = parameters.GetValue<ISource>("source");
		Title = "Source edition";
		SourceTitle = sourceToEdit.Title;
		Details = sourceToEdit.GetDetails();
	}
}
