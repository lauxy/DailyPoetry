using DailyPoetry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetry.Services
{
    public interface IRecentViewItemService
    {

        /// <summary>
        /// 列出所有RecentViews项目。
        /// </summary>
        /// <returns></returns>
        List<RecentViewItem> ListRecentViewItems();

        /// <summary>
        /// 删除RecentViews项目。
        /// </summary>
        /// <param name="recentViewItem"></param>
        /// <returns></returns>
        void DeleteRecentViewItems();
    }
}
