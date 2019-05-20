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
            if (fontIcon.Glyph == "\uEB51")
            {
                fontIcon.Glyph = "\uEB52"; // 换成红心
                fontIcon.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                fontIcon.Glyph = "\uEB51"; // 换成空心
                fontIcon.Foreground = new SolidColorBrush(Colors.Black);
            }
            var poetryItem = (PoetryItem)(e.OriginalSource as ToggleButton).DataContext;
            (DataContext as RecentViewPageViewModel).AddItemsToFavoriteTable(poetryItem.Id);
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

        [Obsolete]
        private void RecentViewPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            (DataContext as RecentViewPageViewModel).RefreshPage();

            var _ListView = RecentViewListView as ListView;
            foreach (PoetryItem item in _ListView.Items)
            {
                var _Container = RecentViewListView.ItemContainerGenerator
                    .ContainerFromItem(item);
                var _Children = AllChildren(_Container);
                var _FavoriteButton = _Children
                    // only interested in TextBlock
                    .OfType<ToggleButton>()
                    // only interested in FirstName
                    .First(x => x.Name.Equals("FavoriteButton"));
                var fontIcon = (_FavoriteButton.Content as FontIcon);
                if ((DataContext as DetailViewModel).GetFavoriteStatus(item.Id))
                {
                    fontIcon.Glyph = "\uEB52"; // 换成红心
                    fontIcon.Foreground = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    fontIcon.Glyph = "\uEB51"; // 换成空心
                    fontIcon.Foreground = new SolidColorBrush(Colors.Black);
                }
            }
        }

        public List<Control> AllChildren(DependencyObject parent)
        {
            var _List = new List<Control>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var _Child = VisualTreeHelper.GetChild(parent, i);
                if (_Child is Control)
                    _List.Add(_Child as Control);
                _List.AddRange(AllChildren(_Child));
            }
            return _List;
        }

    }

      
}
