using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
namespace Mneme.Views.Base;

public abstract class SearchableViewModel<T> : BindableBase
{
	protected SearchableViewModel()
	{
		searchedPhrase = string.Empty;
		FilteredItems = [];
		AllItems = [];
		AllItems.CollectionChanged += AllItems_CollectionChanged;
	}
	private string searchedPhrase;

	public string SearchedPhrase
	{
		get => searchedPhrase;
		set
		{
			_ = SetProperty(ref searchedPhrase, value);
			if (value.Length > 2)
			{
				UpdateFilteredItems();
				RaisePropertyChanged(nameof(FilteredItems));
			} else
			{
				FilteredItems = AllItems;
				RaisePropertyChanged(nameof(FilteredItems));
			}
		}
	}
	protected void UpdateFilteredItems() => FilteredItems = new ObservableCollection<T>(AllItems.Where(SearchCondition()));

	protected abstract Func<T, bool> SearchCondition();

	public ObservableCollection<T> AllItems { get; private set; }
	private void AllItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
	{
		if (SearchedPhrase.Length == 0)
			FilteredItems = AllItems;
		RaisePropertyChanged(nameof(FilteredItems));
	}
	public ObservableCollection<T> FilteredItems { get; private set; }
}
