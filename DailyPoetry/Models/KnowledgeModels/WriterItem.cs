using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetry.Models
{
    [Table("authors")]
    public class WriterItem
    {
        public int Id { get; set; }
        public int Name { get; set; }
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
    }
}
