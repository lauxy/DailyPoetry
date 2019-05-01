using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DailyPoetry.Models.KnowledgeModels;

namespace DailyPoetry.Models
{
    public class UserContext : DbContext
    {
        /// <summary>
        /// “最近浏览”数据表。
        /// </summary>
        public DbSet<BrowseRecordItem> BrowseRecordItems { get; set; }

        /// <summary>
        /// “我最喜欢的诗词”数据表。
        /// </summary>
        public DbSet<BrowseRecordItem> FavoriteItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=user.sqlite");
        }

        [Table("BrowseRecord")]
        public class BrowseRecordItem
        {
            public PoetryItem poetryItem { get; set; }
            public string poetryName { get; set; }
            public string poetName { get; set; }
            public string mainContent { get; set; }
        }
    }
}


