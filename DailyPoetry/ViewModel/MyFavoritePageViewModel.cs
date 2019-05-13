using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DailyPoetry.Models.KnowledgeModels;
using DailyPoetry.Services;
using GalaSoft.MvvmLight;

namespace DailyPoetry.ViewModel
{
    public class MyFavoritePageViewModel : ViewModelBase
    {
        private KnowledgeService _knowledgeService;

        private ObservableCollection<PoetryItem> _favoriteItems;

        public MyFavoritePageViewModel(KnowledgeService knowledgeService)
        {
            _knowledgeService = knowledgeService;
            _knowledgeService.Entry();
            _favoriteItems = new ObservableCollection<PoetryItem>();
            var a = _knowledgeService._knowledgeContext.FavoriteItems.ToList();
            foreach (var favoriteItem in a)
            {
                _favoriteItems.Add(_knowledgeService.GetPoetryItemById(favoriteItem.PoetryId));
            }
        }

        public ObservableCollection<PoetryItem> FavoriteItems
        {
            get => _favoriteItems;
            set => Set(nameof(FavoriteItems), ref _favoriteItems, value);
        }
    }
}
