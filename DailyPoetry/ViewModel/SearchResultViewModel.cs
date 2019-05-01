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

        private IEnumerator<PoetryIntermediateType> poetryIntermediate;

        private List<PoetryItem> poetryItems;

        public List<PoetryItem> PoetryItems {
            get => poetryItems;
            set => Set(nameof(PoetryItems), ref poetryItems, value);
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
            poetryIntermediate = null;
            poetryItems = new List<PoetryItem>();
        }

        public void AddFilter()
        {
            filterItems.Add(new FilterItem(FilterCategory.CONTENT, ""));
            Debug.Write(FilterItems.Count());
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

        public FilterItem(FilterCategory filterCategory, string value)
        {
            index = -1;
            this.filterCategory = filterCategory;
            this.value = value;
        }
    }
}
