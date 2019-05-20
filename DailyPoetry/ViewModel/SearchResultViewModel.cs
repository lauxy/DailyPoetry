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
using System.Threading;
using System.Collections.Specialized;
using System.Collections;
using Windows.System;

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

        private PoetryIntermediateType _poetryIntermediate;

        // visibility, process ring`s active, button`s enabled

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

        private Visibility _poetryResultVisibility;

        public Visibility PoetryResultVisibility
        {
            get => _poetryResultVisibility;
            set => Set(nameof(PoetryResultVisibility), ref _poetryResultVisibility, value);
        }

        private Visibility _resultNavigateBarVisibility;

        public Visibility ResultNavigateBarVisibility
        {
            get => _resultNavigateBarVisibility;
            set => Set(nameof(ResultNavigateBarVisibility), ref _resultNavigateBarVisibility, value);
        }

        private Visibility _noResultTipVisibility;

        public Visibility NoResultTipVisibility
        {
            get => _noResultTipVisibility;
            set => Set(nameof(NoResultTipVisibility), ref _noResultTipVisibility, value);
        }

        private bool _processRingActive;

        public bool ProcessRingActive
        {
            get => _processRingActive;
            set => Set(nameof(ProcessRingActive), ref _processRingActive, value);
        }

        private bool _prevButtonEnabled;

        public bool PrevButtonEnabled
        {
            get => _prevButtonEnabled;
            set => Set(nameof(PrevButtonEnabled), ref _prevButtonEnabled, value);
        }

        private bool _nextButtonEnabled;

        public bool NextButtonEnabled
        {
            get => _nextButtonEnabled;
            set => Set(nameof(NextButtonEnabled), ref _nextButtonEnabled, value);
        }

        public SearchResultViewModel(KnowledgeService knowledgeService)
        {
            _knowledgeService = knowledgeService;
            _knowledgeService.Entry();
            _poetryItems = null;
            FilterListVisibility = Visibility.Visible;
            FilterSimplifiedListVisibility = Visibility.Collapsed;
            FilterItems = new ObservableCollection<FilterItem>();
            FilterItems.Add(new FilterItem(FilterCategory.CONTENT, ""));
            CollapsedFilterItems = new ObservableCollection<FilterItem>();
            NoResultTipVisibility = Visibility.Collapsed;
            _updateFilterIndex();
        }

        // filter`s data

        private ObservableCollection<FilterItem> _filterItems;

        public ObservableCollection<FilterItem> FilterItems
        {
            get => _filterItems;
            set => Set(nameof(FilterItems), ref _filterItems, value);
        }

        private ObservableCollection<FilterItem> _collapsedFilterItems;

        public ObservableCollection<FilterItem> CollapsedFilterItems
        {
            get => _collapsedFilterItems;
            set => Set(nameof(CollapsedFilterItems), ref _collapsedFilterItems, value);
        }

        // result`s data

        private List<PoetryItem> _poetryItems;

        public List<PoetryItem> PoetryItems
        {
            get => _poetryItems;
            set => Set(nameof(PoetryItems), ref _poetryItems, value);
        }

        private List<int> _pageIndex;

        public List<int> PageIndex
        {
            get => _pageIndex;
            set => Set(nameof(PageIndex), ref _pageIndex, value);
        }

        private int _currentPage;

        public int CurrentPage
        {
            get => _currentPage;
            set => Set(nameof(CurrentPage), ref _currentPage, value);
        }

        private int _pageSize = 20;

        private int _pageCnt;

        public int PageCnt
        {
            get => _pageCnt;
            set => Set(nameof(PageCnt), ref _pageCnt, value);
        }

        public bool Refreshing;

        // commands

        private RelayCommand _addFilterCommand;

        public RelayCommand AddFilterCommand =>
        _addFilterCommand ?? (_addFilterCommand = new RelayCommand(() =>
        {
            FilterItems.Add(new FilterItem(FilterCategory.CONTENT, ""));
            _updateFilterIndex();
        }));

        // public RelayCommand DeleteFilterCommand
        // 在 viewmodel 中实现，因为不知道怎么在 command 里带参数
        // 目前在 viewmodel 中读取 button 的 tag 来实现参数

        private RelayCommand _chevronSwitchCommand;

        /// <summary>
        /// 展开收起 filter 的 panel
        /// </summary>
        public RelayCommand ChevronSwitchCommand =>
            _chevronSwitchCommand ?? (_chevronSwitchCommand = new RelayCommand(() =>
            {
                if(_filterListVisibility == Visibility.Visible)
                {
                    _updateCollapsedFilter();
                    FilterListVisibility = Visibility.Collapsed;
                    FilterSimplifiedListVisibility = Visibility.Visible;
                }
                else
                {
                    FilterListVisibility = Visibility.Visible;
                    FilterSimplifiedListVisibility = Visibility.Collapsed;
                }
            }));

        private RelayCommand _searchCommand;

        public RelayCommand SearchCommand =>
            _searchCommand ?? (_searchCommand = new RelayCommand(() => {
                // clean controls` status
                PoetryResultVisibility = Visibility.Collapsed;
                ResultNavigateBarVisibility = Visibility.Collapsed;
                ProcessRingActive = true;

                DoSearch();

                ProcessRingActive = false;
                if (_poetryIntermediate.Count() == 0)
                {
                    NoResultTipVisibility = Visibility.Visible;
                }
                else
                {
                    NoResultTipVisibility = Visibility.Collapsed;
                    PoetryResultVisibility = Visibility.Visible;
                    if (_pageCnt > 1)
                    {
                        ResultNavigateBarVisibility = Visibility.Visible;
                        PrevButtonEnabled = false;
                        NextButtonEnabled = true;
                        PageIndex = Enumerable.Range(1, _pageCnt).ToList();
                    }
                }
                Debug.Write("exed");

            }));

        private RelayCommand _nextPageCommand;

        public RelayCommand NextPageCommand =>
            _nextPageCommand ?? (_nextPageCommand = new RelayCommand(async () =>
            {
                NextPage();
            }));

        private RelayCommand _prevPageCommand;

        public RelayCommand PrevPageCommand =>
            _prevPageCommand ?? (_prevPageCommand = new RelayCommand(async () =>
            {
                PrevPage();
            }));

        private RelayCommand _refreshPageCommand;

        public RelayCommand RefreshPageCommand =>
            _refreshPageCommand ?? (_refreshPageCommand = new RelayCommand(async () =>
            {
                RefreshPage();
            }));

        public RelayCommand _searchFallbackCommand;

        public RelayCommand SearchFallbackCommand =>
            _searchFallbackCommand ?? (_searchFallbackCommand = new RelayCommand(async () =>
            {
                string query = "";
                foreach (var filteritem in FilterItems)
                    query += filteritem.Value;
                var uriBing = new Uri(@"https://cn.bing.com/search?q=" + query);
                var success = await Launcher.LaunchUriAsync(uriBing);
            }));

        // helpers

        private void _updateFilterIndex()
        {
            int i = 0;
            foreach (var filterItem in _filterItems)
            {
                filterItem.Index = i;
                ++i;
            }
        }

        private void _updateCollapsedFilter()
        {
            CollapsedFilterItems.Clear();
            foreach (var filterItem in FilterItems)
                if (filterItem.Value != "")
                    CollapsedFilterItems.Add(filterItem);
        }

        // activity

        public void DeleteFilter(int index)
        {
            FilterItems.RemoveAt(index);
            _updateFilterIndex();
            _updateCollapsedFilter();
        }

        public void UpdateFilterCategory(int index, int new_choice)
        {
            FilterItems[index].FilterCategory = (FilterCategory)new_choice;
        }

        public void NextPage()
        {
            CurrentPage += 1;
            if (CurrentPage == _pageCnt)
                NextButtonEnabled = false;
            PrevButtonEnabled = true;
            PoetryItems = _poetryIntermediate.
                Skip((CurrentPage - 1) * _pageSize).Take(_pageSize).ToListFull();
            

        }

        public void PrevPage()
        {
            CurrentPage -= 1;
            if (CurrentPage == 1)
                PrevButtonEnabled = false;
            NextButtonEnabled = true;
            PoetryItems = _poetryIntermediate.
                Skip((CurrentPage - 1) * _pageSize).Take(_pageSize).ToListFull();
        }

        public void RefreshPage()
        {
            if (CurrentPage == 1)
                PrevButtonEnabled = false;
            if (CurrentPage == _pageCnt)
                NextButtonEnabled = false;
            if(_poetryIntermediate != null)
                PoetryItems = _poetryIntermediate.
                    Skip((CurrentPage-1) * _pageSize).Take(_pageSize).ToListFull();
        }


        public PoetryItem SetContentQuery(string query)
        {
            FilterItems = new ObservableCollection<FilterItem>();
            FilterItems.Add(new FilterItem(FilterCategory.CONTENT, query));
            CollapsedFilterItems = new ObservableCollection<FilterItem>();
            _updateFilterIndex();
            if(SearchCommand.CanExecute(null))
            {
                SearchCommand.Execute(null);
                return PoetryItems?.Count() == 1 ? PoetryItems[0] : null;
            }
            return null;
        }

        public void DoSearch()
        {
            // build path
            _poetryIntermediate = _knowledgeService.GetAllSimplifiedPoetryItems();
            foreach (var filterItem in _filterItems)
            {
                switch (filterItem.FilterCategory)
                {
                    case FilterCategory.TITLE:
                        _poetryIntermediate = _knowledgeService.GetPoetryItemsByName(_poetryIntermediate, filterItem.Value);
                        break;
                    case FilterCategory.CONTENT:
                        _poetryIntermediate = _knowledgeService.GetPoetryItemsByContent(_poetryIntermediate, filterItem.Value);
                        break;
                    case FilterCategory.WRITER:
                        _poetryIntermediate = _knowledgeService.GetPoetryItemsByWriter(_poetryIntermediate, filterItem.Value);
                        break;
                    default:
                        break;
                }
            }

            // set values
            PageCnt = (_poetryIntermediate.Count() + _pageSize - 1) / _pageSize;
            PageCnt = PageCnt > 1 ? PageCnt : 1;
            CurrentPage = 1;
            RefreshPage();
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
