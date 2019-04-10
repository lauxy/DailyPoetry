using DailyPoetry.Models;
using DailyPoetry.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetry.ViewModel
{
    public class RecentViewPageViewModel : ViewModelBase
    {
        /// <summary>
        /// RecentView项目服务。
        /// </summary>
        private IRecentViewItemService _recentViewItemService;

       // RecentViewItemService obj = new RecentViewItemService();

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="recentViewItemService"></param>
        public RecentViewPageViewModel(IRecentViewItemService recentViewItemService)
        {
            _recentViewItemService = recentViewItemService;
            RecentViewItems = _recentViewItemService.ListRecentViewItems();
            Debug.WriteLine("VM Created");
        }

        /// <summary>
        /// 刷新命令。
        /// </summary>
        private RelayCommand _deleteCommand;

        /// <summary>
        /// RecentViewItem项
        /// </summary>
        private List<RecentViewItem> _recentViewItems;

        /// <summary>
        /// RecentViewItem项
        /// </summary>
        public List<RecentViewItem> RecentViewItems
        {
            get => _recentViewItems;
            set => Set(nameof(RecentViewItems), ref _recentViewItems, value);
        }

        /// <summary>
        ///  删除命令。
        /// </summary>
        public RelayCommand DeleteCommand =>
            _deleteCommand ?? (_deleteCommand = new RelayCommand(() =>
            {
                RecentViewItems = new List<RecentViewItem>();
                _recentViewItemService.DeleteRecentViewItems();
            }));

    }
}
