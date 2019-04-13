using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DailyPoetry.Services
{
    public class RecommendItemService : IRecommendItemService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RecommendItemService()
        {
           // JObject tokenJObject = GetUsersToken();
        }

        /// <summary>
        /// 获取每日推荐的诗句。
        /// </summary>
        /// <param name="token">用户Token</param>
        /// <returns>每日推荐的诗句</returns>
        public async Task<RecommendData> GetRecommendContentAsync()
        {
            var httpClient = new HttpClient();
            StorageFile tokenFile = 
                await ApplicationData.Current.LocalFolder.GetFileAsync("UsersToken.dat");
            string header = await FileIO.ReadTextAsync(tokenFile);
            //Add a user-agent header to the GET request. 
            var headers = httpClient.DefaultRequestHeaders;
            headers.Add("X-User-Token", header);

            string jString = "";
            try
            {
                //Send the GET request
                var response = await httpClient.GetAsync("https://v2.jinrishici.com/sentence");
                response.EnsureSuccessStatusCode();
                jString = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                jString = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
            }

            return JsonConvert.DeserializeObject<Rootobject>(jString).data;
        }
    }
}