using DailyPoetry.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace DailyPoetry.Services
{
    public class RecentViewItemService : IRecentViewItemService
    {
        List<RecentViewItem> recentViews;

        public RecentViewItemService() {
            Debug.WriteLine("S Created");
        }

        /// <summary>
        /// 列出所有RecentView项目
        /// </summary>
        /// <returns></returns>
        public List<RecentViewItem> ListRecentViewItems()
        {
            recentViews = new List<RecentViewItem>();
            int id = 1;
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

            try
            {
                foreach (var line in File.ReadLines(storageFolder.Path + "/RecentViews.txt", Encoding.UTF8))
                {
                    string[] item = line.Split(" ");
                    int showLength = item[2].Length > 15 ? 15 : item[2].Length;
                    recentViews.Add(new RecentViewItem
                    {
                        RecentViewId = id,
                        Poet = item[0],
                        Title = item[1],
                        Content = item[2].Substring(0, showLength) + "..."
                    });
                    id++;
                }
            }
            catch (System.IO.FileNotFoundException) //文件不存在，就创建一个文件
            {
                string filePath = storageFolder.Path + "/RecentViews.txt";
                File.Create(filePath);
            }

            return recentViews;
        }

        /// <summary>
        /// 删除所有RecentView项目。
        /// </summary>
        /// <param name="recentViewItem"></param>
        public void DeleteRecentViewItems()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            FileStream fileStream = File.OpenWrite(storageFolder.Path + "/RecentViews.txt");
            fileStream.SetLength(0);
            fileStream.Close();
        }
    }
}
