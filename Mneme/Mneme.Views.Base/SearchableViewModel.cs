using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Mvvm;
namespace Mneme.Views.Base
{
	public abstract class SearchableViewModel<T> : BindableBase
	{
		protected SearchableViewModel()
		{
			searchedPhrase = string.Empty;
			FilteredItems = new ObservableCollection<T>();
			AllItems = new List<T>();
		}
		private string searchedPhrase;

		public string SearchedPhrase
		{
			get => searchedPhrase;
			set
			{
				SetProperty(ref searchedPhrase, value);
				UpdateFilteredItems();
			}
		}
		protected void UpdateFilteredItems()
		{
			if (searchedPhrase.Length > 2)
			{
				FilteredItems = new ObservableCollection<T>(AllItems.Where(SearchCondition()));
			}
			else if (FilteredItems.Count != AllItems.Count)
			{
				FilteredItems = new ObservableCollection<T>(AllItems);
			}
		}

		protected abstract Func<T, bool> SearchCondition();

		private List<T> allItems;
		public List<T> AllItems
		{
			get => allItems;
			set
			{
				SetProperty(ref allItems, value);
				UpdateFilteredItems();
			}
		}
		private ObservableCollection<T> filteredItems;
		public ObservableCollection<T> FilteredItems
		{
			get => filteredItems;
			set => SetProperty(ref filteredItems, value);
		}
	}
}
