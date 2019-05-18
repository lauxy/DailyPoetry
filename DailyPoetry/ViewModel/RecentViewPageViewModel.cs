using DailyPoetry.Models.KnowledgeModels;
using DailyPoetry.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DailyPoetry.ViewModel
{
    public class RecentViewPageViewModel : ViewModelBase
    {
        private KnowledgeService _knowledgeService;

        private ObservableCollection<PoetryItem> _recentViewItems;

        /// <summary>
        /// 构造函数。
        /// </summary>
        public RecentViewPageViewModel(KnowledgeService knowledgeService)
        {
            _knowledgeService = knowledgeService;
            _knowledgeService.Entry();
            _recentViewItems = new ObservableCollection<PoetryItem>();
            var a = _knowledgeService._knowledgeContext.RecentViewItems.ToList();
            foreach (var recentViewItem in a)
            {
                _recentViewItems.Add(_knowledgeService.GetPoetryItemById(recentViewItem.PoetryItemId));
            }
            _knowledgeService.PoetryIsLikedTagger(ref _recentViewItems);
            _knowledgeService.Dispose(); //手动释放资源
        }

        /// <summary>
        /// 删除命令。
        /// </summary>
        private RelayCommand _deleteCommand;

        /// <summary>
        /// 将poetryItem添加到喜爱的诗词数据库。
        /// </summary>
        private RelayCommand _likeCommand;

        /// <summary>
        /// RecentViewItems 最近浏览记录的get/set方法
        /// </summary>
        public ObservableCollection<PoetryItem> RecentViewItems
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
                
            }));

        public RelayCommand LikeCommand =>
            _likeCommand ?? (_likeCommand = new RelayCommand(() =>
            {
                
            }));

    }
}
