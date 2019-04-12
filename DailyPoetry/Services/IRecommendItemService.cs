using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DailyPoetry.Services
{
    /// <summary>
    /// 每日推荐服务接口类。
    /// </summary>
    public interface IRecommendItemService
    {
        Task<RecommendData> GetRecommendContentAsync();
    }


    public class Rootobject
    {
        public string status { get; set; }
        public RecommendData data { get; set; }
        public string token { get; set; }
        public string ipAddress { get; set; }
        public object warning { get; set; }
    }

    public class RecommendData
    {
        public string id { get; set; }
        public string content { get; set; }
        public int popularity { get; set; }
        public Origin origin { get; set; }
        public string[] matchTags { get; set; }
        public string recommendedReason { get; set; }
        public DateTime cacheAt { get; set; }
    }

    public class Origin
    {
        public string title { get; set; }
        public string dynasty { get; set; }
        public string author { get; set; }
        public string[] content { get; set; }
        public object translate { get; set; }
    }
}
