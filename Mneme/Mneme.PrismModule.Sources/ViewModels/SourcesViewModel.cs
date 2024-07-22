using MaterialDesignThemes.Wpf;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Model;
using Mneme.PrismModule.Sources.Views;
using Mneme.Sources;
using Mneme.Views.Base;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Mneme.PrismModule.Sources.ViewModels;

internal class SourcesViewModel : SearchableViewModel<Source>, INavigationAware
{
	private readonly ISnackbarMessageQueue snackbarMessageQueue;
	private readonly IDialogService dialogService;
	private readonly SourcesManager manager;
	private CancellationTokenSource cts;

	private Source selectedSource;
	public Source SelectedSource
	{
		get => selectedSource;
		set => SetProperty(ref selectedSource, value);
	}

	private bool isLoading;
	public bool IsLoading
	{
		get => isLoading;
		set => SetProperty(ref isLoading, value);
	}

	private readonly bool sourcesListEmpty;
	public bool SourcesListEmpty => AllItems.Count == 0 && !isLoading;

	public SourcesViewModel(ISnackbarMessageQueue snackbarMessageQueue, IDialogService dialogService, SourcesManager manager) : base()
	{
		this.snackbarMessageQueue = snackbarMessageQueue;
		this.dialogService = dialogService;
		this.manager = manager;
		IgnoreSourceCommand = new DelegateCommand<Source>(IgnoreSource, (x) => x?.Active ?? false);
		ActivateSourceCommand = new DelegateCommand<Source>(ActivateSource, (x) => !x?.Active ?? false);
		DeleteSourceCommand = new DelegateCommand<Source>(DeleteSource);
		EditSourceCommand = new DelegateCommand<Source>(EditSource, (x) => x?.TextType == MnemeSource.Type);
		ShowDialogCreateSourceCommand = new DelegateCommand(CreateSource);
		IsLoading = true;
	}

	private void EditSource(Source source)
	{
		var param = new DialogParameters
			{
				{ "source", source}
			};
		dialogService.ShowDialog(nameof(SourceCreationView), param, result =>
		{
			if (result.Result == ButtonResult.OK)
			{
				Source editedSource = result.Parameters.GetValue<Source>("source");
				var index = AllItems.IndexOf(source);
				AllItems.RemoveAt(index);
				AllItems.Insert(index, editedSource);
				SelectedSource = editedSource;
			}
		});
	}

	private async void DeleteSource(Source source)
	{
		if (await manager.DeleteSource(source))
		{
			_ = AllItems.Remove(source);
			RaisePropertyChanged(nameof(SourcesListEmpty));
		}
		else
			snackbarMessageQueue.Enqueue("Could not delete source. Some related data to this source still exists.");
	}

	private async void IgnoreSource(Source source)
	{
		Source updatedSource = await manager.IgnoreSource(source);
		_ = AllItems.Remove(source);
		AllItems.Add(updatedSource);
		SelectedSource = updatedSource;
	}

	private async void ActivateSource(Source source)
	{
		Source updatedSource = await manager.ActivateSource(source);
		_ = AllItems.Remove(source);
		AllItems.Add(updatedSource);
		SelectedSource = updatedSource;
	}

	public DelegateCommand<Source> IgnoreSourceCommand { get; set; }
	public DelegateCommand<Source> ActivateSourceCommand { get; set; }
	public DelegateCommand<Source> EditSourceCommand { get; set; }
	public DelegateCommand<Source> DeleteSourceCommand { get; set; }
	public DelegateCommand ShowDialogCreateSourceCommand { get; }

	private void CreateSource()
	{
		dialogService.ShowDialog(nameof(SourceCreationView), null, result =>
		{
			if (result.Result == ButtonResult.OK)
			{
				MnemeSource source = result.Parameters.GetValue<MnemeSource>("source");
				AllItems.Add(source);
				RaisePropertyChanged(nameof(SourcesListEmpty));
			}
		});
	}

	public async void OnNavigatedTo(NavigationContext navigationContext)
	{
		using (cts = new CancellationTokenSource())
		{
			IsLoading = true;
			Task<IReadOnlyList<Source>> getSourcesTask = manager.GetSourcesPreviewAsync(cts.Token);
			Task completedTask = await Task.WhenAny(getSourcesTask, Task.Delay(Timeout.Infinite, cts.Token));

			if (completedTask == getSourcesTask)
			{
				IReadOnlyList<Source> sources = getSourcesTask.Result;
				if (sources.Count != AllItems.Count)
				{
					AllItems.Clear();
					_ = AllItems.AddRange(sources);
				}
				IsLoading = false;
				RaisePropertyChanged(nameof(SourcesListEmpty));
			}
		}
		cts = null;
	}

	public bool IsNavigationTarget(NavigationContext navigationContext) => true;

	public void OnNavigatedFrom(NavigationContext navigationContext)
	{
		if (AllItems.Count != 0)
			cts?.Cancel();
	}

	protected override Func<Source, bool> SearchCondition() => x => x.Title.ToLower().Contains(SearchedPhrase.ToLower()) || x.TextType.ToLower() == SearchedPhrase.ToLower();
}
