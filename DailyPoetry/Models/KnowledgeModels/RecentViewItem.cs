using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetry.Models.KnowledgeModels
{
    [Table("recent_view")]
    public class RecentViewItem
    {
        public int Id { get; set; }
        public int PoetryItemId { get; set; }
    }
}
