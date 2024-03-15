using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;

namespace ODP.ViewModels
{
	public class FilterViewModelCollection<FilterT, T> : GenericViewModelList<FilterT, T>
		where T:BaseFilterViewModel<FilterT>
	{
		public static readonly DependencyProperty SelectAllCommandProperty = DependencyProperty.Register("SelectAllCommand", typeof(ViewModelCommand), typeof(FilterViewModelCollection<FilterT, T>), new PropertyMetadata(null));
		public ViewModelCommand SelectAllCommand
		{
			get { return (ViewModelCommand)GetValue(SelectAllCommandProperty); }
			set { SetValue(SelectAllCommandProperty, value); }
		}
		public static readonly DependencyProperty SelectNoneCommandProperty = DependencyProperty.Register("SelectNoneCommand", typeof(ViewModelCommand), typeof(FilterViewModelCollection<FilterT, T>), new PropertyMetadata(null));
		public ViewModelCommand SelectNoneCommand
		{
			get { return (ViewModelCommand)GetValue(SelectNoneCommandProperty); }
			set { SetValue(SelectNoneCommandProperty, value); }
		}

		public FilterViewModelCollection() : base(new List<FilterT>())
		{
			SelectAllCommand = new ViewModelCommand(SelectAllCanExecute, SelectAllExecute);
			SelectNoneCommand = new ViewModelCommand(SelectNoneCanExecute, SelectNoneExecute);
		}
		private bool SelectAllCanExecute(object? arg)
		{
			return true;
		}

		private void SelectAllExecute(object? obj)
		{
			foreach (T item in this) item.IsSelected = true;
		}
		private bool SelectNoneCanExecute(object? arg)
		{
			return true;
		}

		private void SelectNoneExecute(object? obj)
		{
			foreach (T item in this) item.IsSelected = false;
		}

		public override int GetNewItemIndex(T Item)
		{
			Comparer<string> comparer = Comparer<string>.Default;

			for(int t=0;t<Count;t++)
			{
				if (comparer.Compare(Item.Name , this[t].Name)<0) return t;
			}
			return Count;
		}

		protected override T OnCreateItem(FilterT SourceItem)
		{
			throw new NotImplementedException();
		}

	}
}
