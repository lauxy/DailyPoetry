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

        private ViewModelLocator()
        {
            SimpleIoc.Default.Register<IRecentViewItemService, RecentViewItemService>();
            SimpleIoc.Default.Register<RecentViewPageViewModel>();
            //SimpleIoc.Default.Register<RecentViewPageViewModel>();
        }

    }
}
