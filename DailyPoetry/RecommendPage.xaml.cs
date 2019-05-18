using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using DailyPoetry.Models.KnowledgeModels;
using DailyPoetry.Services;


// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace DailyPoetry
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class RecommendPage : Page
    {
        // get user's settings info
        private static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        // time stamp
        string prefix = DateTime.Today.ToShortDateString().Replace("/", "");
        // get local folder 
        StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
        /// <summary>
        /// Init Page
        /// </summary>
        public RecommendPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Page Loading.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void RecommendPage_OnLoading(FrameworkElement sender, object args)
        {
            GridLoading.Visibility = Visibility.Visible;
            ProgressRingLoading.IsActive = true;

            GenerateBgService generateBgService = new GenerateBgService();
            try
            {
                // 显示背景
                RecommendPageBg.ImageSource = await generateBgService.GetBitmapImageAsync();
                // 显示诗句
                DataContext = ViewModelLocator.Instance.RecommendPageViewModel;

                string text = ShowPoetryArea.Text;


                if (localSettings.Values["SetWallpaper"].Equals(true))
                {
                    // 用户允许将每日推荐图片设置为壁纸
                    await generateBgService.CreateBackgroundImageAsync(text);
                    WallpaperService wallpaperService = new WallpaperService();
                    if (!await wallpaperService.WallpaperChanger())
                    {
                        // Todo: 当设置壁纸失败时异常处理。
                    }
                }
                else if (localSettings.Values["SetLockScreen"].Equals(true))
                {
                    // 用户允许将每日推荐图片设置为锁屏
                    await generateBgService.CreateBackgroundImageAsync(text);
                    WallpaperService wallpaperService = new WallpaperService();
                    if (!await wallpaperService.LockScreenChanger())
                    {
                        // Todo: 当设置锁屏失败时异常处理。
                    }
                }
            }
            catch (Exception imgOrTxtException)
            {
                TextBlockLoading.FontSize = 20;

                TextBlockLoading.Text = "ExceptionInfo: " + imgOrTxtException.Message +
                                        "\n抱歉，这里出现了一些错误, 请您将页面截图后反馈给我们 " +
                                        "\n我们会及时做出修改并将在下一个版本中改进该问题 " +
                                        "\n以便使我们的软件能够带给您更好的使用体验 \n谢谢！";
            }

            ProgressRingLoading.IsActive = false;
            GridLoading.Visibility = Visibility.Collapsed;
        }

        private void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SearchResultPage), ShowPoetryArea.Text);
        }

        private async void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequestDeferral deferral = args.Request.GetDeferral();
            args.Request.Data.Properties.Title = "Sharing Title";
            args.Request.Data.Properties.Description = "分享每日诗词";
            args.Request.Data.SetText("我正在赏析 “" + ShowPoetryArea.Text + "”");
            var imageUri = "ms-appdata:///local/" + prefix + "Wallpaper.jpg";

            args.Request.Data.SetBitmap(
                Windows.Storage.Streams.RandomAccessStreamReference.CreateFromUri(new Uri(imageUri)));
            deferral.Complete();
        }

        /// <summary>
        /// 分享每日诗词卡片。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ShareButton_OnClick(object sender, RoutedEventArgs e)
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            if (File.Exists(storageFolder.Path + @"\" + prefix + "Wallpaper.jpg"))
            {
                dataTransferManager.DataRequested += DataTransferManager_DataRequested;
                DataTransferManager.ShowShareUI();
            }
            else
            {
                ContentDialog noFileDialog = new ContentDialog()
                {
                    Title = "温馨提示：",
                    Content = "分享文件不存在，请先在设置页将图片设置为壁纸，再执行此操作！",
                    CloseButtonText = "Ok"
                };
                await noFileDialog.ShowAsync();
            }
        }
    }
}