using DailyPoetry.Models.KnowledgeModels;
using DailyPoetry.Services;
using DailyPoetry.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    /// todo: 跳转后导航到top
    /// </summary>
    public sealed partial class DetailPage : Page
    {
        private PoetryItem poetryItem;

        public DetailPage()
        {
            this.InitializeComponent();
            DataContext = ViewModelLocator.Instance.DetailViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            poetryItem = (PoetryItem)e.Parameter;
            Title.Text = poetryItem.Name;
            Content.Text = poetryItem.Content;
            Dynasty.Text = poetryItem.Dynasty;
            Writer.Text = poetryItem.AuthorName;
            Annotation.Text = TextGuard(poetryItem?.Annotation);
            Translation.Text = TextGuard(poetryItem?.Translation, OptimizeChineseText);
            Appreciation.Text = TextGuard(poetryItem?.Appreciation, OptimizeChineseText);

            if (poetryItem.Layout == "center")
                Content.HorizontalTextAlignment = TextAlignment.Center;
            else if (poetryItem.Layout == "indent")
                Content.HorizontalTextAlignment = TextAlignment.Left;
            else
            {
                LayoutNotDefinedTip.IsOpen = true;
            }

            (DataContext as DetailViewModel).RecordRecentView(poetryItem.Id);
            var fontIcon = (FavoriteButton.Content as FontIcon);
            if ((DataContext as DetailViewModel).GetFavoriteStatus(poetryItem.Id))
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

        private string OptimizeChineseText(string rawString)
        {
            var splitted = rawString.Split('\n');
            string result = "    " + splitted[0];
            foreach(string part in splitted.Skip(1))
            {
                result += "\n    " + part;
            }
            return result;
        }

        private string TextGuard(string rawString, Func<string, string> callback = null, string fallback="暂无数据")
        {
            if (rawString == null || rawString == "")
                return fallback;
            if (callback == null)
                return rawString;
            return callback(rawString);
        }

        /// <summary>
        /// 点击心形按钮，将item加入“我喜欢的诗词”中
        /// copy from RecentViewPage.xaml.cs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppBarToggleButton_Click(object sender, RoutedEventArgs e)
        {
            // 每点击一次切换图标
            // todo：在数据库更新完成后设置
            ToggleButton b = sender as ToggleButton;
            var fontIcon = (b.Content as FontIcon);
            if((DataContext as DetailViewModel).SwitchFavoriteStatus(poetryItem.Id))
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

        private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequestDeferral deferral = args.Request.GetDeferral();
            args.Request.Data.Properties.Title = "分享诗词";
            args.Request.Data.Properties.Description = "《" + poetryItem.Name + "》";

            args.Request.Data.SetText("我正在赏析诗人" + poetryItem.AuthorName + "的《" + poetryItem.Name + "》。\n" + poetryItem.Content);
            deferral.Complete();
        }

        /// <summary>
        /// 分享每日词话卡片。
        /// copy from RecommandPage.xaml.cs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShareButton_OnClick(object sender, RoutedEventArgs e)
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
            DataTransferManager.ShowShareUI();

        }

        private void FontList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as DetailViewModel).UpdateFont(FontList.SelectedIndex);
        }
    }
}
