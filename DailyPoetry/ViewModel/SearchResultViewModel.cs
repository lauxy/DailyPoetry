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

        public bool _processRingVisibility;

        public bool ProcessRingVisibility
        {
            get => _processRingVisibility;
            set => Set(nameof(ProcessRingVisibility), ref _processRingVisibility, value);
        }

        private Visibility _poetryResultVisibility;

        public Visibility PoetryResultVisibility
        {
            get => _poetryResultVisibility;
            set => Set(nameof(PoetryResultVisibility), ref _poetryResultVisibility, value);
        }

        private ObservableCollection<FilterItem> filterItems;

        public ObservableCollection<FilterItem> FilterItems
        {
            get => filterItems;
            set => Set(nameof(FilterItems), ref filterItems, value);
        }

        public RelayCommand _addFilterCommand;

        public RelayCommand AddFilterCommand =>
        _addFilterCommand ?? (_addFilterCommand = new RelayCommand(() =>
        {
            FilterItems.Add(new FilterItem(FilterCategory.CONTENT, ""));
            _updateFilterIndex();
        }));

        public RelayCommand _chevronSwitchCommand;

        public RelayCommand ChevronSwitchCommand =>
            _chevronSwitchCommand ?? (_chevronSwitchCommand = new RelayCommand(() =>
            {
                var tmpVisiblity = _filterSimplifiedListVisibility;
                FilterSimplifiedListVisibility = _filterListVisibility;
                FilterListVisibility = tmpVisiblity;
                _updateFilterVisibility();
            }));

        public void DeleteFilter(int index)
        {
            FilterItems.RemoveAt(index);
            _updateFilterIndex();
        }

        public void UpdateFilterCategory(int index, int new_choice)
        {
            FilterItems[index].FilterCategory = (FilterCategory)new_choice;
        }

        private RelayCommand _searchCommand;

        public RelayCommand SearchCommand =>
            _searchCommand ?? (_searchCommand = new RelayCommand(async () =>
            {
                PoetryResultVisibility = Visibility.Collapsed;
                ProcessRingVisibility = true;
                PoetryIntermediateType result = _knowledgeService.GetAllSimplifiedPoetryItems();
                foreach (var filterItem in filterItems)
                {
                    switch (filterItem.FilterCategory)
                    {
                        case FilterCategory.TITLE:
                            result = _knowledgeService.GetPoetryItemsByName(result, filterItem.Value);
                            break;
                        case FilterCategory.CONTENT:
                            result = _knowledgeService.GetPoetryItemsByContent(result, filterItem.Value);
                            break;
                        case FilterCategory.WRITER:
                            result = _knowledgeService.GetPoetryItemsByWriter(result, filterItem.Value);
                            break;
                        default:
                            break;
                    }
                }
                PoetryItems = await result.ToListFullAsync();
                ProcessRingVisibility = false;
                PoetryResultVisibility = Visibility.Visible;
            }));

        private void _updateFilterIndex()
        {
            int i = 0;
            foreach (var filterItem in filterItems)
            {
                filterItem.Index = i;
                ++i;
            }
        }

        private void _updateFilterVisibility()
        {
            foreach (var filterItem in filterItems)
            {
                filterItem.visibility = filterItem.Value == "" ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public SearchResultViewModel(KnowledgeService knowledgeService)
        {
            _knowledgeService = knowledgeService;
            _knowledgeService.Entry();
            _poetryItems = new List<PoetryItem>();
            FilterListVisibility = Visibility.Visible;
            FilterSimplifiedListVisibility = Visibility.Collapsed;
            FilterItems = new ObservableCollection<FilterItem>();
            FilterItems.Add(new FilterItem(FilterCategory.CONTENT, ""));
            _updateFilterIndex();
        }
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
                        resultCount = (uint)result.Count();
                        await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            foreach (var item in result)
                            {
                                this.Add(item);
                            }
                        });
                    }

                    return new LoadMoreItemsResult() { Count = resultCount };
                }).AsAsyncOperation<LoadMoreItemsResult>();
        }
    }

    public enum FilterCategory
    {
        TITLE, CONTENT, WRITER
    };

    public class FilterItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _index;
        private FilterCategory _filterCategory;
        private string _value;
        private Visibility _visibility;

        public int Index
        {
            get => _index;
            set
            {
                _index = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Index"));
            }
        }

        public FilterCategory FilterCategory
        {
            get => _filterCategory;
            set
            {
                _filterCategory = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FilterCategory"));
            }
        }

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }

        public Visibility visibility
        {
            get => _visibility;
            set
            {
                _visibility = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("visibility"));
            }
        }

        public FilterItem(FilterCategory filterCategory, string value)
        {
            Index = 0;
            FilterCategory = filterCategory;
            Value = value;
        }
    }

    public class FilterCategoryStringFormater : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            switch (value)
            {
                case FilterCategory.TITLE: return "标题";
                case FilterCategory.CONTENT: return "内容";
                case FilterCategory.WRITER: return "作者";
                default: throw new InvalidCastException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            switch (value)
            {
                case "标题": return FilterCategory.TITLE;
                case "内容": return FilterCategory.CONTENT;
                case "作者": return FilterCategory.WRITER;
                default: throw new InvalidCastException();
            }
        }
    }
}
