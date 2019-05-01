using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
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
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Graphics.Canvas;
using Image = DailyPoetry.Services.Image;

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
    }
}
