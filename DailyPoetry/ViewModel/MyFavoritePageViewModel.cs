using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
            RefreshPage();
        }

        public ObservableCollection<PoetryItem> FavoriteItems
        {
            get => _favoriteItems;
            set => Set(nameof(FavoriteItems), ref _favoriteItems, value);
        }

        public void RefreshPage()
        {
            using (_knowledgeService.Entry())
            {
                FavoriteItems = new ObservableCollection<PoetryItem>();
                var a = _knowledgeService._knowledgeContext.FavoriteItems.ToList();
                foreach (var favoriteItem in a)
                {
                    FavoriteItems.Add(_knowledgeService.GetPoetryItemById(favoriteItem.PoetryId));
                }
                Debug.WriteLine(FavoriteItems.Count());
            }
        }

        public void DeleteFavoritePoetry(int poetryId)
        {
            using (_knowledgeService.Entry())
            {
                _knowledgeService.DeleteFavoriteItemByPoetryIdItem(poetryId);
                FavoriteItems.Remove(FavoriteItems.Single(fi => fi.Id == poetryId));
            }
        }
    }
}
