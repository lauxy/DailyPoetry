using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetry.Models.KnowledgeModels
{
    /// <summary>
    /// 这个表描述每一个作品属于的类别，比如“望岳”的类别为“唐诗三百首”，“山”等
    /// 注意每个作品可能对应多个标签
    /// </summary>
    public class CategoryWorkItem
    {
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        public string Category { get; set; }
        [Column("author_id")] public int AuthorId { get; set; }
        [Column("author_name")] public int AuthorName { get; set; }
        [Column("work_id")] public int WorkId { get; set; }
        [Column("work_name")] public string WorkName { get; set; }
        [Column("work_dynasty")] public string WorkDynasty { get; set; }
        [Column("work_type")] public string WorkType { get; set; }
    }

}
