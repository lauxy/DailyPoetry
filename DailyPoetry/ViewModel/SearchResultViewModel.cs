using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using DailyPoetry.Models.KnowledgeModels;
using DailyPoetry.Services;
using GalaSoft.MvvmLight;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp;
using System.ComponentModel;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Xaml.Controls;

namespace DailyPoetry.ViewModel
{
    // using statements are strictly per-file. There is no simple way to share
    // alias cross files. 
    using PoetryIntermediateType = IntermediateResult<PoetryItem, SimplifiedPoetryItem>;
    using WriterIntermediateType = IntermediateResult<WriterItem, SimplifiedWriterItem>;
    using CategoryIntermediateType = IntermediateResult<CategoryItem, SimplifiedCategoryItem>;

    public class SearchResultViewModel : ViewModelBase
    {
        private KnowledgeService _knowledgeService;

        private List<PoetryItem> _poetryItems;

        public List<PoetryItem> PoetryItems
        {
            get => _poetryItems;
            set => Set(nameof(PoetryItems), ref _poetryItems, value);
        }

        private Visibility _filterListVisibility;

        public Visibility FilterListVisibility
        {
            get => _filterListVisibility;
            set => Set(nameof(FilterListVisibility), ref _filterListVisibility, value);
        }
        private Visibility _filterSimplifiedListVisibility;

        public Visibility FilterSimplifiedListVisibility
        {
            get => _filterSimplifiedListVisibility;
            set => Set(nameof(FilterSimplifiedListVisibility), ref _filterSimplifiedListVisibility, value);
        }

        private ObservableCollection<FilterItem> filterItems = new ObservableCollection<FilterItem> {
                new FilterItem(FilterCategory.TITLE, ""),
                new FilterItem(FilterCategory.CONTENT, "")};

        public ObservableCollection<FilterItem> FilterItems {
            get => filterItems;
            set => Set(nameof(FilterItems), ref filterItems, value);
        }

        public SearchResultViewModel(KnowledgeService knowledgeService)
        {
            _knowledgeService = knowledgeService;
            _knowledgeService.Entry();
            _poetryItems = new List<PoetryItem>();
            FilterListVisibility = Visibility.Visible;
            FilterSimplifiedListVisibility = Visibility.Collapsed;
        }

        public RelayCommand _addFilterCommand;

        public RelayCommand AddFilterCommand =>
        _addFilterCommand ?? (_addFilterCommand = new RelayCommand(() =>
        {
            filterItems.Add(new FilterItem(FilterCategory.CONTENT, ""));
        }));

        public RelayCommand<object> _chevronSwitchCommand;

        public RelayCommand<object> ChevronSwitchCommand =>
        _chevronSwitchCommand ?? (_chevronSwitchCommand = new RelayCommand<object>((object sender) =>
        {
            var tmpVisiblity = _filterSimplifiedListVisibility;
            FilterSimplifiedListVisibility = _filterListVisibility;
            FilterListVisibility = tmpVisiblity;
            //((sender as Button).DataContext as SearchResultPage).SwitchChevron();
        }));

    }

    public class IncrementalLoadingCollection<T, I> : ObservableCollection<I>,
        ISupportIncrementalLoading
        where T : IIncrementalSource<I>, new()
    {
        private T source;
        private int itemsPerPage;
        private bool hasMoreItems;
        private int currentPage = 0;

        public IncrementalLoadingCollection(T source, int itemsPerPage = 10)
        {
            this.source = source;
            this.itemsPerPage = itemsPerPage;
            hasMoreItems = true;
        }

        public bool HasMoreItems => hasMoreItems;

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            var dispatcher = Window.Current.Dispatcher;
            return Task.Run<LoadMoreItemsResult>(
                async () =>
                {
                    uint resultCount = 0;
                    var result = await source.GetPagedItemsAsync(
                        currentPage++, (int)count);
                    if (result == null || !result.Any())
                    {
                        hasMoreItems = false;
                    }
                    else
                    {
                        resultCount = (uint) result.Count();
                        await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            foreach (var item in result)
                            {
                                this.Add(item);
                            }
                        });
                    }

                    return new LoadMoreItemsResult() {Count = resultCount};
                }).AsAsyncOperation<LoadMoreItemsResult>();
        }
    }

    public enum FilterCategory
    {
        TITLE, CONTENT, WRITER, DYSTANY
    };

    public class FilterItem
    {
        public int index;
        public FilterCategory filterCategory;
        public string value;

        public Visibility visibility { get =>  (value == "") ? Visibility.Collapsed : Visibility.Visible; }

        public FilterItem(FilterCategory filterCategory, string value)
        {
            index = -1;
            this.filterCategory = filterCategory;
            this.value = value;
        }
    }
}
