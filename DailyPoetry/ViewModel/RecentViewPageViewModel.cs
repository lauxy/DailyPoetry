using DailyPoetry.Models.KnowledgeModels;
using DailyPoetry.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.Store.Preview.InstallControl;

namespace DailyPoetry.ViewModel
{
    public class RecentViewPageViewModel : ViewModelBase
    {
        private KnowledgeService _knowledgeService;

        private ObservableCollection<PoetryItem> _recentViewItems;

        //private ObservableCollection<int> _order;

        /// <summary>
        /// 构造函数。
        /// </summary>
        public RecentViewPageViewModel(KnowledgeService knowledgeService)
        {
            _knowledgeService = knowledgeService;
            RefreshPage();
        }

        

        /// <summary>
        /// RecentViewItems 最近浏览记录的get/set方法
        /// </summary>
        public ObservableCollection<PoetryItem> RecentViewItems
        {
            get => _recentViewItems;
            set => Set(nameof(RecentViewItems), ref _recentViewItems, value);
        }

        //public ObservableCollection<int> Order
        //{
        //    get => _order;
        //    set => Set(nameof(Order), ref _order, value);
        //}

        /// <summary>
        /// 删除最近浏览记录
        /// </summary>
        /// <param name="poetryId"></param>
        public void DeleteRecentViewItem(int poetryId)
        {
            using (_knowledgeService.Entry())
            {
                _knowledgeService.DeleteRecentViewItemByPoetryIdItem(poetryId);
                RecentViewItems.Remove(RecentViewItems.Single(fi => fi.Id == poetryId));
            }
        }

        public void RefreshPage()
        {
            using (_knowledgeService.Entry())
            {
                RecentViewItems = new ObservableCollection<PoetryItem>();
                var recentViewCollection = _knowledgeService._knowledgeContext.RecentViewItems.ToList();
                foreach (var recentViewItem in recentViewCollection)
                {
                    RecentViewItems.Add(_knowledgeService.GetPoetryItemById(recentViewItem.PoetryItemId));
                }
                //Order = new ObservableCollection<int>();
                //foreach (var i in Enumerable.Range(1, RecentViewItems.Count))
                //{
                //    Order.Add(i);
                //}
                _knowledgeService.PoetryIsLikedTagger(ref _recentViewItems);
            }
        }

        public void AddItemsToFavoriteTable(int poetryId)
        {
            using (_knowledgeService.Entry())
            {
                _knowledgeService.AddFavoriteItem(poetryId);
            }
        }

      
    }
}
