using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace DailyPoetry
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class RecentViewPage : Page
    {
        public RecentViewPage()
        {
            this.InitializeComponent();
            GenerateRecordingList();
        }

        /// <summary>
        /// 读日志文件，生成记录列表
        /// </summary>
        private void GenerateRecordingList()
        {
            foreach (var line in File.ReadLines("./Logs/sample.txt", Encoding.UTF8))
            {
                ListViewItem item = new ListViewItem();
                item.Content = line;
                RecordListView.Items.Add(item);
            }
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
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

       
    }
}
