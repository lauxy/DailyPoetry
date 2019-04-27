using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DailyPoetry.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;

namespace DailyPoetry.ViewModel
{
    public class RecommendPageViewModel : ViewModelBase
    {
        private ITokenService _tokenService;
        private IRecommendItemService _recommendItemService;
        private ILocalInfoService _localInfoService;
       // private IBingImageService _bingImageService;

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="tokenService"></param>
        /// <param name="recommendItemService"></param>
        /// <param name="localInfoService"></param>
        public RecommendPageViewModel(ITokenService tokenService,
            IRecommendItemService recommendItemService,
            ILocalInfoService localInfoService)
        {
            _tokenService = tokenService;
            _recommendItemService = recommendItemService;
            _localInfoService = localInfoService;
            //_bingImageService = bingImageService;
            TokenData key = Task.Run(_tokenService.GetUsersToken).Result;
            RecommendItems = Task.Run(_recommendItemService.GetRecommendContentAsync).Result;
            LocalInfoItems = Task.Run(_localInfoService.GetLocalInfoAsync).Result;
            //BingImageItems = Task.Run(_bingImageService.getBingImageAsync).Result;
        }

        /// <summary>
        /// 推荐诗句。
        /// </summary>
        private RecommendData _recommendItems = new RecommendData();

        /// <summary>
        /// RecommendData项。
        /// </summary>
        public RecommendData RecommendItems
        {
            get => _recommendItems;
            set => Set(nameof(RecommendItems), ref _recommendItems, value);
        }

        /// <summary>
        /// 当地天气、时间、Tag的数据。
        /// </summary>
        private LocalInfoData _localInfoItems = new LocalInfoData();

        /// <summary>
        /// LocalInfoData项。
        /// </summary>
        public LocalInfoData LocalInfoItems
        {
            get => _localInfoItems;
            set => Set(nameof(LocalInfoItems), ref _localInfoItems, value);
        }

        //private BingImageData _bingImageItems = new BingImageData();

        //public BingImageData BingImageItems
        //{
        //    get => _bingImageItems;
        //    set => Set(nameof(BingImageItems), ref _bingImageItems, value);
        //}
    }
}
