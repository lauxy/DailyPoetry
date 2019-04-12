using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json.Linq;

namespace DailyPoetry.Services
{
    public class TokenService : ITokenService
    {
        public async Task<TokenData> GetUsersToken()
        {
            var httpClient = new HttpClient();
            string json = "";
            //创建一个文件保存用户的Token码
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

            try
            {
                //Send the GET request 
                var response = await httpClient.GetAsync("https://v2.jinrishici.com/token");
                response.EnsureSuccessStatusCode();
                json = await response.Content.ReadAsStringAsync();
                JObject jObject = JObject.Parse(json); // convert jString to JObject

                string filePath = storageFolder.Path + "/UsersToken.dat";
                StorageFile tokenFile;
                if (!File.Exists(filePath)) //文件不存在就创建一个文件
                {
                    tokenFile = 
                        await storageFolder.CreateFileAsync("UsersToken.dat",
                        Windows.Storage.CreationCollisionOption.ReplaceExisting);
                }
                else //文件存在
                {
                    tokenFile = await storageFolder.GetFileAsync("UsersToken.dat");
                    var size = await tokenFile.GetBasicPropertiesAsync();
                    if (size.Size > 0)
                    {
                        //用户已获得Token密钥直接返回即可
                        TokenData res = new TokenData();
                        res.status = "success";
                        res.data = await FileIO.ReadTextAsync(tokenFile);
                        return res;
                    }
                }
                //文件为空，未能从文件中获取用户Token，需要将新的Token写入文件中
                await FileIO.WriteTextAsync(tokenFile, jObject["data"].ToString());
            }
            catch (Exception ex)
            {
                json = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
            }

            return JsonConvert.DeserializeObject<TokenData>(json);
        }
    }
}
