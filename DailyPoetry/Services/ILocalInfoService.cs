using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetry.Services
{
    public interface ILocalInfoService
    {
        Task<LocalInfoData> GetLocalInfoAsync();
    }

    public class LocalInfoObject
    {
        public string status { get; set; }
        public LocalInfoData data { get; set; }
    }

    public class LocalInfoData
    {
        public string token { get; set; }
        public string ip { get; set; }
        public string region { get; set; }
        public Weatherdata weatherData { get; set; }
        public string[] tags { get; set; }
        public DateTime beijingTime { get; set; }
    }

    public class Weatherdata
    {
        public float temperature { get; set; }
        public string windDirection { get; set; }
        public int windPower { get; set; }
        public float humidity { get; set; }
        public string updateTime { get; set; }
        public string weather { get; set; }
        public string visibility { get; set; }
        public float rainfall { get; set; }
        public float pm25 { get; set; }
    }

}
