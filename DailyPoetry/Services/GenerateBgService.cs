using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml.Media.Imaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DailyPoetry.Services
{
    public class GenerateBgService : IBingImageService
    {
        /// <summary>
        /// This Method fetches the JSON string from the BingAPI endpoint and then returns the Raw String to the caller.
        /// We need to use either void or Task if we use await in the method body.
        /// </summary>
        /// <param name="_numOfImages"></param>
        /// <returns></returns>
        public async Task<BingImageData> getBingImageAsync(int _numOfImages)
        {
            // We can specify the region we want for the Bing Image of the Day.
            string strRegion = "en-US";
            string strBingImageURL =
                string.Format("http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n={0}&mkt={1}", _numOfImages,
                    strRegion);
            string strJSONString = "";

            // Use Windows.Web.Http Namespace as System.Net.Http will be deprecated in future versions.
            HttpClient httpClient = new HttpClient();
            string json = await httpClient.GetStringAsync(new Uri(strBingImageURL));

            // strJson Deserialize to Json class
            BingImageData bingImageData = JsonConvert.DeserializeObject<BingImageData>(json);
            return bingImageData;
        }

       /// <summary>
       /// This Method parses the fetched JSON string and retrieves the Image URLs using NewtonSoft Json.Net
       /// Each Url is stored as a separate List item and the list of URLs is returned to the caller.
       /// </summary>
       /// <param name="_numOfImages"></param>
       /// <param name="_strRawJSONString"></param>
       /// <returns></returns>
        public List<string> parseJSONString_Newtonsoft(int _numOfImages, string _strRawJSONString)
        {
            List<string> _lstBingImageURLs = new List<string>(_numOfImages);

            JObject jResults = JObject.Parse(_strRawJSONString);
            foreach (var image in jResults["images"])
            {
                _lstBingImageURLs.Add((string)image["url"]);
            }
            
            return _lstBingImageURLs;
        }


        /// <summary>
        /// 将文字（诗句）和图片（配图）合成一张图片，作为壁纸或锁屏。
        /// </summary>
        /// <param name="imageName">原配图的名字</param>
        /// <param name="text">诗句的内容</param>
        /// <returns></returns>
        public async Task CreateBackgroundImageAsync(string imageName, string text)
        {
            string filepath = "ms-appx:///Assets/" + imageName;
            Uri imageuri = new Uri(filepath);
            StorageFile inputFile = await StorageFile.GetFileFromApplicationUriAsync(imageuri);
            BitmapDecoder imagedecoder;
            using (var imagestream = await inputFile.OpenAsync(FileAccessMode.Read))
            {
                imagedecoder = await BitmapDecoder.CreateAsync(imagestream);
            }
            CanvasDevice device = CanvasDevice.GetSharedDevice();
            CanvasRenderTarget renderTarget = new CanvasRenderTarget(device, imagedecoder.PixelWidth, imagedecoder.PixelHeight, 96);
            using (var ds = renderTarget.CreateDrawingSession())
            {
                ds.Clear(Colors.White);
                CanvasBitmap image = await CanvasBitmap.LoadAsync(device, inputFile.Path, 96);
                ds.DrawImage(image);
                ds.DrawText(text, new System.Numerics.Vector2(150, 150), Colors.Black);
            }
            string filename = "Wallpaper.png";
            StorageFolder pictureFolder = KnownFolders.SavedPictures;
            var file = await pictureFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                await renderTarget.SaveAsync(fileStream, CanvasBitmapFileFormat.Png, 1f);
            }
        }
    }
}
