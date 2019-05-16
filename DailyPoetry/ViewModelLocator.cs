using DailyPoetry.Services;
using DailyPoetry.ViewModel;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetry
{
    public class ViewModelLocator
    {
        public static readonly ViewModelLocator Instance =
            new ViewModelLocator();

        public RecentViewPageViewModel RecentViewPageViewModel =>
            SimpleIoc.Default.GetInstance<RecentViewPageViewModel>();

        public RecommendPageViewModel RecommendPageViewModel =>
            SimpleIoc.Default.GetInstance<RecommendPageViewModel>();

        public SearchResultViewModel SearchResultViewModel =>
            SimpleIoc.Default.GetInstance<SearchResultViewModel>();

        public MyFavoritePageViewModel MyFavoritePageViewModel =>
            SimpleIoc.Default.GetInstance<MyFavoritePageViewModel>();

        public DetailViewModel DetailViewModel => 
            SimpleIoc.Default.GetInstance<DetailViewModel>();

        private ViewModelLocator()
        {
            SimpleIoc.Default.Register<ITokenService, TokenService>();
            SimpleIoc.Default.Register<IRecommendItemService, RecommendItemService>();
            SimpleIoc.Default.Register<ILocalInfoService, LocalInfoService>();
            SimpleIoc.Default.Register<IBingImageService,GenerateBgService>();
            SimpleIoc.Default.Register<RecentViewPageViewModel>();
            SimpleIoc.Default.Register<RecommendPageViewModel>();
            SimpleIoc.Default.Register<MyFavoritePageViewModel>();

            SimpleIoc.Default.Register<KnowledgeService>();
            SimpleIoc.Default.Register<SearchResultViewModel>(
                () => new SearchResultViewModel(SimpleIoc.Default.GetInstance<KnowledgeService>("Search")));
            SimpleIoc.Default.Register<DetailViewModel>(
                () => new DetailViewModel(SimpleIoc.Default.GetInstance<KnowledgeService>("Detail")));
        }

    }
}
