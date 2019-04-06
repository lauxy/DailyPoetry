using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetry.Models
{
    public class RecentView
    {
        public int RecentViewId { get; set; } //最近浏览的ID
        public string Poet { get; set; }      //诗人姓名
        public string Title { get; set; }     //诗词的标题
        public string Content { get; set; }   //诗词的代表性内容
    }

    public class RecentViewManager
    {
        public static List<RecentView> GetRecentViews()
        {
            var recentViews = new List<RecentView>();

            int id = 1;

            foreach (var line in File.ReadLines("./Logs/sample.txt", Encoding.UTF8))
            {
                string[] item = line.Split(" ");
                recentViews.Add(new RecentView {
                    RecentViewId = id,
                    Poet = item[0],
                    Title = item[1],
                    Content = item[2].Substring(0, 15) + "..."
                });
                
                id++;
            }

            return recentViews;
        }
    }
}
