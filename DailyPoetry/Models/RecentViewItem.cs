using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace DailyPoetry.Models
{
    public class RecentViewItem
    {
        public int RecentViewId { get; set; } //最近浏览的ID
        public string Poet { get; set; }      //诗人姓名
        public string Title { get; set; }     //诗词的标题
        public string Content { get; set; }   //诗词的代表性内容
    }
}
