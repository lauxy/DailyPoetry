using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using DailyPoetry.Services;
using Microsoft.EntityFrameworkCore.Migrations;
using Image = DailyPoetry.Services.Image;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace DailyPoetry
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class RecommendPage : Page
    {
        public RecommendPage()
        {
            this.InitializeComponent();
        }

        private async void RecommendPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            GridLoading.Visibility = Visibility.Visible;
            ProgressRingLoading.IsActive = true;
            DataContext = ViewModelLocator.Instance.RecommendPageViewModel;

            GenerateBgService generateBgService = new GenerateBgService();
            RecommendPageBg.ImageSource = await generateBgService.GetPageBackground();

            ProgressRingLoading.IsActive = false;
            GridLoading.Visibility = Visibility.Collapsed;
        }
    }
}
