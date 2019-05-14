using DailyPoetry.Models.KnowledgeModels;
using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
        /// 点击记录项执行的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedItem = (RecentViewItem)e.ClickedItem; //获取当前被点击的对象的引用

        }

        /// <summary>
        /// 点击心形按钮，将item加入“我喜欢的诗词”中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppBarToggleButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarToggleButton b = sender as AppBarToggleButton;
            var poetryItem = (PoetryItem)(e.OriginalSource as Button).DataContext;
            if (b != null)
            {
                 

            }
        }
        
    }

      
}
