using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DailyPoetry.Models
{
    public class UserContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=user.sqlite");
        }

        /// <summary>
        /// “最近浏览”数据表。
        /// </summary>
        public DbSet<BrowseRecordItem> BrowseRecordItems { get; set; }

        /// <summary>
        /// “我最喜欢的诗词”数据表。
        /// </summary>
        public DbSet<BrowseRecordItem> FavoriteItems { get; set; }
    }
}


