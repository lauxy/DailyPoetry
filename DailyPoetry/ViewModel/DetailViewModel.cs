using DailyPoetry.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetry.ViewModel
{
    public class DetailViewModel
    {
        private KnowledgeService _knowledgeService;
        public DetailViewModel(KnowledgeService knowledgeService)
        {
            _knowledgeService = knowledgeService;
        }

        public void RecordRecentView(int PoetryId)
        {
            using (_knowledgeService.Entry())
            {
                _knowledgeService.AddRecentViewItem(PoetryId);
            }
        }
    }
}
