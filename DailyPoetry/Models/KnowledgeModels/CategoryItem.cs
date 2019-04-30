using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetry.Models.KnowledgeModels
{
    /// <summary>
    /// 这个表描述了诗词的类别，比如“唐诗三百首”，“诗经全集”，“悼亡”，“春”
    /// </summary>
    public class CategoryItem
    {
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        public string Name { get; set; }
        public string Slogan { get; set; }
        public string Intro { get; set; }
        public string Library { get; set; }
        public string Press { get; set; }
        [Column("works_count")] public int WorksCount { get; set; }
        [Column("rhesises_count")] public int RhesisesCount { get; set; }
    }
}
