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
using Windows.Storage;
using DailyPoetry.Models.KnowledgeModels;
using System.Threading.Tasks;

namespace DailyPoetry
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SearchResultPage : Page
    {
        private static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        private PoetryItem navigateTmp;

        public SearchResultPage()
        {
            this.InitializeComponent();
            navigateTmp = null;
            DataContext = ViewModelLocator.Instance.SearchResultViewModel;
            (DataContext as SearchResultViewModel).ResultNavigateBarVisibility = Visibility.Collapsed;
            (DataContext as SearchResultViewModel).NoResultTipVisibility = Visibility.Collapsed;
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigateTmp = null;
            if (e.NavigationMode == NavigationMode.Back)
                return;
            var query = e.Parameter as string;
            if (query == null)
                return;
            var searchResult = (DataContext as SearchResultViewModel).SetContentQuery(query);
            navigateTmp = searchResult;
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


        private void PageIndex_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (DataContext == null)
                return;
            (DataContext as SearchResultViewModel).RefreshPage();
        }

        private void SearchResultList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var poetryItem = e.ClickedItem;
            Frame.Navigate(typeof(DetailPage), poetryItem);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (navigateTmp != null)
            {
                var tmp = navigateTmp;
                navigateTmp = null;
                Frame.Navigate(typeof(DetailPage), tmp);
            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var viewModel = (SearchResultViewModel)DataContext;
            if (viewModel.SearchCommand.CanExecute(null))
                viewModel.SearchCommand.Execute(null);
        }
    }

}
