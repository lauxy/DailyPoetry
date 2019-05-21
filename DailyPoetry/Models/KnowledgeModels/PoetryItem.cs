using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyPoetry.Models.KnowledgeModels
{
    /// <summary>
    /// 使用前务必浏览 SimplifiedPoetryItem
    /// </summary>
    [Table("works")]
    public class PoetryItem : IDbItem<SimplifiedPoetryItem>
    {
        public int Id { get; set; }
        [NotMapped]
        public int Order { get; set; }
        public string Name { get; set; }
        [Column("rhesises_count")]
        public string RhesisesCount { get; set; }
        [Column("categories_count")]
        public string CategoriesCount { get; set; }  // 属于的种类计数，在
        [Column("author_name")]
        public string AuthorName { get; set; }
        [Column("author_id")]
        public string AuthorId { get; set; }  // 作者ID
        public string Dynasty { get; set; }  // 作者朝代
        public string Type { get; set; }  // 诗/词/曲
        public string Foreword { get; set; }  // 前言（好像只有桃花潭记有）
        public string Content { get; set; }  // 正文
        public string Intro { get; set; }  // 简介（背景）
        public string Annotation { get; set; }  // 注释
        public string Translation { get; set; }  // 译文
        public string Appreciation { get; set; }  // 赏析
        [Column("famous_reviews")]
        public string FamousReviews { get; set; }  // 评论
        public string Layout { get; set; }  // 显示布局
        [NotMapped]
        private string _abstract = null;

        [NotMapped]
        public string Abstract
        {
            get
            {
                if (_abstract != null)
                    return _abstract;
                return _abstract = Content.Split("。")[0];
            }
        }

        [NotMapped]
        public bool IsLiked = false;

        public SimplifiedPoetryItem ToSimplified()
        {
            return new SimplifiedPoetryItem
            {
                Id = this.Id,
                Name = this.Name,
                Dynasty = this.Dynasty,
                AuthorName = this.AuthorName,
                Abstract = this.Abstract
            };
        }
    }

    /// <summary>
    /// 所有查询语句都会返回 SimplifiedPoetryItem，为节约内存
    /// 如需要 PoetryItem，请使用 KnowledgeService 转换
    /// [NotMapped] 只是预防性的，应该没有意义
    /// </summary>
    [NotMapped]
    public class SimplifiedPoetryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Dynasty { get; set; }
        public string AuthorName { get; set; }
        public string Abstract { get; set; }
    }
}
