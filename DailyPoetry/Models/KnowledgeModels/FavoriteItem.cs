using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyPoetry.Models.KnowledgeModels
{
    [Table("favorite")]
    public class FavoriteItem
    {
        public int Id { get; set; }
        public int PoetryId { get; set; }
    }
}
