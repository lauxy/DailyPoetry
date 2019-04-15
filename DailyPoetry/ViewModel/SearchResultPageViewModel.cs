using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DailyPoetry.Models;
using DailyPoetry.Services;
using GalaSoft.MvvmLight;

namespace DailyPoetry.ViewModel
{
    public class SearchResultPageViewModel : ViewModelBase
    {
        // todo: IService
        private DatabaseService _databaseService;
        private SearchService _searchService;

        public SearchResultPageViewModel(
            DatabaseService databaseService,
            SearchService searchService)
        {
            _databaseService = databaseService;
            _searchService = searchService;
            _poetryItems = _databaseService.GetAllPoetryData();
        }

        private List<PoetryItem> _poetryItems;

        public List<PoetryItem> PoetryItems
        {
            get => _poetryItems;
            set => Set(nameof(PoetryItem), ref _poetryItems, value);
        }
    }
}
