using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetry.Models.KnowledgeModels
{
    [Table("authors")]
    public class WriterItem : IDbItem<SimplifiedWriterItem>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Intro { get; set; }
        [Column("rhesises_count")]
        public int RhesisesCount { get; set; }
        public string Dynasty { get; set; }
        [Column("yob")] public int YearOfBirth { get; set; }
        [Column("yod")] public int YearOfDeath { get; set; }
        [Column("works_count")] public int WorksCount { get; set; }
        [Column("shi_count")] public int ShiCount { get; set; }
        [Column("ci_count")] public int CiCount { get; set; }
        [Column("qu_count")] public int QuCount { get; set; }
        [Column("fu_count")] public int FuCount { get; set; }

        public SimplifiedWriterItem ToSimplified()
        {
            return new SimplifiedWriterItem
            {
                Id = this.Id,
                Dynasty = this.Dynasty,
                Name = this.Name
            };
        }
    }

    [NotMapped]
    public class SimplifiedWriterItem
    {
        public int Id { get; set; }
        public string Dynasty { get; set; }
        public string Name { get; set; }
    }
}
