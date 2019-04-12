using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;

namespace DailyPoetry.Services
{
    public class LocalInfoService : ILocalInfoService
    {
        public async Task<LocalInfoData> GetLocalInfoAsync()
        {
            var httpClient = new HttpClient();
            //Add a user-agent header to the GET request. 
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
                var response = await httpClient.GetAsync("https://v2.jinrishici.com/info");
                response.EnsureSuccessStatusCode();
                jString = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                jString = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
            }

            return JsonConvert.DeserializeObject<LocalInfoObject>(jString).data;
        }
    }
}
