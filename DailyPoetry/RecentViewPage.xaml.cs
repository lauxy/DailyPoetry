using DailyPoetry.Models.KnowledgeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using DailyPoetry.ViewModel;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace DailyPoetry
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class RecentViewPage : Page
    {
        private static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        /// <summary>
        /// 构造函数
        /// </summary>
        public RecentViewPage()
        {
            this.InitializeComponent();
            DataContext = ViewModelLocator.Instance.RecentViewPageViewModel;
            //初始化界面主题
            if (localSettings.Values["ThemeStyle"] != null)
            {
                string theme = localSettings.Values["ThemeStyle"].ToString();
                if (theme == "Dark")
                    this.RequestedTheme = ElementTheme.Dark;
                else if (theme == "Light")
                    this.RequestedTheme = ElementTheme.Light;
                else this.RequestedTheme = ElementTheme.Default;
            }
        }

        /// <summary>
        /// 点击心形按钮，将item加入“我喜欢的诗词”中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FavoriteButton_OnClick(object sender, RoutedEventArgs e)
        {
            // 每点击一次切换图标
            // todo：在数据库更新完成后设置
            ToggleButton b = sender as ToggleButton;
            var fontIcon = (b.Content as FontIcon);
            var poetryItem = (PoetryItem)(e.OriginalSource as ToggleButton).DataContext;
            if (fontIcon.Glyph == "\uEB51")
            {
                fontIcon.Glyph = "\uEB52"; // 换成红心
                fontIcon.Foreground = new SolidColorBrush(Colors.Red);
                (DataContext as RecentViewPageViewModel).AddItemsToFavoriteTable(poetryItem.Id);
            }
            else
            {
                fontIcon.Glyph = "\uEB51"; // 换成空心
                fontIcon.Foreground = new SolidColorBrush(Colors.Black);
                (DataContext as RecentViewPageViewModel).DeleteItemsFromFavoriteTable(poetryItem.Id);
            }
            if (b != null)
            {

            }
        }

        /// <summary>
        /// 点击记录项执行的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedItem = (PoetryItem)e.ClickedItem; //获取当前被点击的对象的引用
            Frame.Navigate(typeof(DetailPage), selectedItem);
        }

        private void DeleteToggleButton_OnClick(object sender, RoutedEventArgs e)
        {
            var deleteItem = (PoetryItem)(e.OriginalSource as ToggleButton).DataContext;
            (DataContext as RecentViewPageViewModel).DeleteRecentViewItem(deleteItem.Id);
            (DataContext as RecentViewPageViewModel).RefreshPage();
        }

        private void RecentViewPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            (DataContext as RecentViewPageViewModel).RefreshPage();
        }

        private void FavoriteButton_OnLoaded(object sender, RoutedEventArgs e)
        {
            ToggleButton button = sender as ToggleButton;
            PoetryItem poetryItem = button.DataContext as PoetryItem;
            if (poetryItem == null)
                return;
            var fontIcon = (button.Content as FontIcon);
            using ((DataContext as RecentViewPageViewModel)._knowledgeService.Entry())
            {
                poetryItem.IsLiked =
                    (DataContext as RecentViewPageViewModel)._knowledgeService.PoetryIsLiked(poetryItem.Id);
            }
            if (poetryItem.IsLiked)
            {
                fontIcon.Glyph = "\uEB52";
                fontIcon.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                fontIcon.Glyph = "\uEB51";
                fontIcon.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
    }
}
