using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DailyPoetry.Models.KnowledgeModels
{

    public class KnowledgeContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite("Data Source=db.sqlite3");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PoetryItem>().ToTable("works");

        }
        public DbSet<PoetryItem> PoetryItems { get; set; }
        public DbSet<CategoryItem> CategoryItems { get; set; }
        public DbSet<CategoryWorkItem> CategoryWorkItems { get; set; }
        public DbSet<WriterItem> WriterItems { get; set; }
        public DbSet<RecentViewItem> RecentViewItems { get; set; }
        public DbSet<FavoriteItem> FavoriteItems { get; set; }

        [DbFunction("instr")]
        public static int SatisfiesMyUserFunction(string x, string y)
        {
            throw new Exception(); // this code doesn't get executed; the call is passed through to the database function
        }
    }
}
