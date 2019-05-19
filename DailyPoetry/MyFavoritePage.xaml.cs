using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DailyPoetry.Models.KnowledgeModels;
using DailyPoetry.Services;
using DailyPoetry.ViewModel;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace DailyPoetry
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MyFavoritePage : Page
    {
        private static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public MyFavoritePage()
        {
            this.InitializeComponent();
            DataContext = ViewModelLocator.Instance.MyFavoritePageViewModel;
            // 初始化界面主题
            if (localSettings.Values["ThemeStyle"] != null)
            {
                string theme = localSettings.Values["ThemeStyle"].ToString();
                if (theme == "Dark")
                    this.RequestedTheme = ElementTheme.Dark;
                else if (theme == "Light")
                    this.RequestedTheme = ElementTheme.Light;
                else this.RequestedTheme = ElementTheme.Default;
            }

            this.NavigationCacheMode = NavigationCacheMode.Disabled;
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedItem = (PoetryItem)e.ClickedItem; //获取当前被点击的对象的引用
            Frame.Navigate(typeof(DetailPage), selectedItem);
        }

        private void MyFavoritePage_OnLoaded(object sender, RoutedEventArgs e)
        {
            (DataContext as MyFavoritePageViewModel).RefreshPage();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            // 每点击一次切换图标
            // todo：在数据库更新完成后设置
            ToggleButton b = sender as ToggleButton;
            var fontIcon = (b.Content as FontIcon);
            if (fontIcon.Glyph == "\uEB52")
            {
                fontIcon.Glyph = "\uEB51"; // 换成空心
                fontIcon.Foreground = new SolidColorBrush(Colors.Black);
            }
            var poetryItem = (PoetryItem)(e.OriginalSource as ToggleButton).DataContext;
            (DataContext as MyFavoritePageViewModel).DeleteFavoritePoetry(poetryItem.Id);
            if (b != null)
            {


            }
        }
    }
}
