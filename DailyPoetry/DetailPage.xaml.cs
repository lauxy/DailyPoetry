using DailyPoetry.Models.KnowledgeModels;
using DailyPoetry.ViewModel;
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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace DailyPoetry
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DetailPage : Page
    {
        public DetailPage()
        {
            this.InitializeComponent();
            DataContext = ViewModelLocator.Instance.DetailViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            PoetryItem poetryItem = (PoetryItem)e.Parameter;
            Title.Text = poetryItem.Name;
            Content.Text = poetryItem.Content;
            Dynasty.Text = poetryItem.Dynasty;
            Writer.Text = poetryItem.AuthorName;
            Annotation.Text = poetryItem.Annotation;
            Translation.Text = poetryItem.Translation;

            if (poetryItem.Layout == "center")
                Content.HorizontalAlignment = HorizontalAlignment.Center;
            else if (poetryItem.Layout == "indent")
                Content.HorizontalAlignment = HorizontalAlignment.Left;
            else
            {
                LayoutNotDefinedTip.IsOpen = true;
            }

            (DataContext as DetailViewModel).RecordRecentView(poetryItem.Id);
        }
    }
}
