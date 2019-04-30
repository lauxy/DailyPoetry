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
using DailyPoetry.Models;
using DailyPoetry.Services;
using GalaSoft.MvvmLight;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp;

namespace DailyPoetry.ViewModel
{
    public class SearchResultViewModel : ViewModelBase
    {
        private KnowledgeService _knowledgeService;
        public SearchResultViewModel(KnowledgeService knowledgeService)
        {
            _knowledgeService = knowledgeService;
            var a = _knowledgeService.GetOneById(12);
            Debug.WriteLine("Yeah");
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
}
