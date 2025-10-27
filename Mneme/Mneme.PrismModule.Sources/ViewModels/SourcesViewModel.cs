using MaterialDesignThemes.Wpf;
using Mneme.Integrations.Mneme.Contract;
using Mneme.Model;
using Mneme.PrismModule.Sources.Views;
using Mneme.Sources;
using Mneme.Views.Base;
using Prism.Commands;
using Prism.Dialogs;
using Prism.Navigation.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Mneme.PrismModule.Sources.ViewModels;

public class SourcesViewModel : SearchableViewModel<ISource>, INavigationAware
{
	private readonly ISnackbarMessageQueue snackbarMessageQueue;
	private readonly IDialogService dialogService;
	private readonly ISourcesFacade sourcesFacade;
	private readonly MnemeSourceProxy mnemeProxy;
	private CancellationTokenSource cts;

	private ISource selectedSource;
	public ISource SelectedSource
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

	public SourcesViewModel(ISnackbarMessageQueue snackbarMessageQueue, IDialogService dialogService, ISourcesFacade facade, MnemeSourceProxy mnemeProxy) : base()
	{
		this.snackbarMessageQueue = snackbarMessageQueue;
		this.dialogService = dialogService;
		sourcesFacade = facade;
		this.mnemeProxy = mnemeProxy;
		IgnoreSourceCommand = new DelegateCommand<ISource>(IgnoreSource, (x) => x?.Active ?? false);
		ActivateSourceCommand = new DelegateCommand<ISource>(ActivateSource, (x) => !x?.Active ?? false);
		DeleteSourceCommand = new DelegateCommand<ISource>(DeleteSource);
		EditSourceCommand = new DelegateCommand<ISource>(EditSource, (x) => x?.TextType == MnemeSource.Type);
		ShowDialogCreateSourceCommand = new DelegateCommand(CreateSource);
		IsLoading = true;
	}

	private void EditSource(ISource source)
	{
		var param = new DialogParameters
			{
				{ "source", source}
			};
		dialogService.ShowDialog(nameof(SourceCreationView), param, result =>
		{
			if (result.Result == ButtonResult.OK)
			{
				var editedSource = result.Parameters.GetValue<ISource>("source");
				var index = AllItems.IndexOf(source);
				AllItems.RemoveAt(index);
				AllItems.Insert(index, editedSource);
				SelectedSource = editedSource;
			}
		});
	}

	private async void DeleteSource(ISource source)
	{
		if (await mnemeProxy.DeleteSource(source))
		{
			_ = AllItems.Remove(source);
			RaisePropertyChanged(nameof(SourcesListEmpty));
		} else
			snackbarMessageQueue.Enqueue("Could not delete source. Some related data to this source still exists.");
	}

	private async void IgnoreSource(ISource source)
	{
		var updatedSource = await sourcesFacade.IgnoreSource(source);
		_ = AllItems.Remove(source);
		AllItems.Add(updatedSource);
		SelectedSource = updatedSource;
	}

	private async void ActivateSource(ISource source)
	{
		var updatedSource = await sourcesFacade.ActivateSource(source);
		_ = AllItems.Remove(source);
		AllItems.Add(updatedSource);
		SelectedSource = updatedSource;
	}

	public DelegateCommand<ISource> IgnoreSourceCommand { get; set; }
	public DelegateCommand<ISource> ActivateSourceCommand { get; set; }
	public DelegateCommand<ISource> EditSourceCommand { get; set; }
	public DelegateCommand<ISource> DeleteSourceCommand { get; set; }
	public DelegateCommand ShowDialogCreateSourceCommand { get; }

	private void CreateSource() => dialogService.ShowDialog(nameof(SourceCreationView), null, result =>
																	{
																		if (result.Result == ButtonResult.OK)
																		{
																			var source = result.Parameters.GetValue<MnemeSource>("source");
																			AllItems.Add(source);
																			RaisePropertyChanged(nameof(SourcesListEmpty));
																		}
																	});

	public async void OnNavigatedTo(NavigationContext navigationContext)
	{
		using (cts = new CancellationTokenSource())
		{
			IsLoading = true;
			var getSourcesTask = sourcesFacade.GetSourcesPreviewAsync(cts.Token);
			var completedTask = await Task.WhenAny(getSourcesTask, Task.Delay(Timeout.Infinite, cts.Token));

			if (completedTask == getSourcesTask)
			{
				var sources = getSourcesTask.Result;
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

	protected override Func<ISource, bool> SearchCondition() => x => x.Title.ToLower().Contains(SearchedPhrase.ToLower()) || x.TextType.ToLower() == SearchedPhrase.ToLower();
}
