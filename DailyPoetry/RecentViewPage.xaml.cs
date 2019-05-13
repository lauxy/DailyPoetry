using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Microsoft.Win32;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DailyPoetry.Models;
using Windows.Storage;
using System.Collections.ObjectModel;
using DailyPoetry.ViewModel;
using DailyPoetry.Services;

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
            //var selectedItem = (RecentViewItem)e.ClickedItem; //获取当前被点击的对象的引用

        }

        /// <summary>
        /// 点击心形按钮，将item加入“我喜欢的诗词”中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppBarToggleButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarToggleButton b = sender as AppBarToggleButton;
            if (b != null)
            {
                throw new NotImplementedException();        

            }
        }
        
    }

      
}
