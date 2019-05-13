using System;
using System.Collections.Generic;
using System.IO;
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
using DailyPoetry.ViewModel;
using System.Diagnostics;
using DailyPoetry.Models.KnowledgeModels;
using System.Threading.Tasks;

namespace DailyPoetry
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SearchResultPage : Page
    {
        public SearchResultPage()
        {
            this.InitializeComponent();
            DataContext = ViewModelLocator.Instance.SearchResultViewModel;
            (DataContext as SearchResultViewModel).ResultNavigateBarVisibility = Visibility.Collapsed;
        }

        private void ChevronButton_Click(object sender, RoutedEventArgs e)
        {
            if (ChevronIcon.Glyph == "\uE70E") // xaml 和 c# 表示十六进制不一样
            {
                ChevronIcon.Glyph = "\uE70D";
                ChevronText.Text = "展开";
            }
            else
            {
                ChevronIcon.Glyph = "\uE70E";
                ChevronText.Text = "收起";
            }
            AddButton.IsEnabled = !AddButton.IsEnabled;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as SearchResultViewModel).DeleteFilter((int)(sender as Button).Tag);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var combobox = (sender as ComboBox);
            if (combobox.Tag == null)
                return;
            (DataContext as SearchResultViewModel).UpdateFilterCategory((int)combobox.Tag, combobox.SelectedIndex);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var poetryItem = (PoetryItem)(e.OriginalSource as Button).DataContext;
            Frame.Navigate(typeof(DetailPage), poetryItem);
        }

        private void PageIndex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as SearchResultViewModel).RefreshPage();
        }
    }

}
