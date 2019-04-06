using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
using static DailyPoetry.ChangeWallpaper;
using DailyPoetry.Models;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace DailyPoetry
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class RecentViewPage : Page
    {

        private List<RecentView> RecentViews;

        public RecentViewPage()
        {
            this.InitializeComponent();
            RecentViews = RecentViewManager.GetRecentViews();
        }

        /// <summary>
        /// “全选”复选框被选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// “全选”复选框未被选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 删除最近浏览的记录信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            
            //测试更换壁纸
            //ChangeWallpaper changer = new ChangeWallpaper();
            //bool x = await changer.SetWallpaperAsync("Buildings.jpg");
            ChangeWallpaper changer = new ChangeWallpaper();
            bool x = await changer.WallpaperChanger();

            if (x == true)
            {
                TestArea.Text = Application.Current.ToString();
            }
            else TestArea.Text = "No";
        }

        /// <summary>
        /// 点击记录项执行的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedItem = (RecentView)e.ClickedItem; //获取当前被点击的对象的引用

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
