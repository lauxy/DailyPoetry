using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetry.Models
{
    [Table("BrowseRecord")]
    public class BrowseRecordItem
    {
        public PoetryItem poetryItem { get; set; }
        public string poetryName { get; set; }
        public string poetName { get; set; }
        public string mainContent { get; set; }
    }
}
