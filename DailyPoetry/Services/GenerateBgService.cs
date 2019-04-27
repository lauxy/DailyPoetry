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
using Windows.Storage.Streams;
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
        /// 获取Bing每日图片的Json，反序列化为类。
        /// </summary>
        /// <returns>返回BingImageData类</returns>
        public async Task<BingImageData> getBingImageAsync()
        {
            // We can specify the region we want for the Bing Image of the Day.
            string strRegion = "en-US";
            string strBingImageURL =
                string.Format("http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n={0}&mkt={1}", 1,
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
        /// 获取Bing图片。
        /// </summary>
        /// <returns>返回BitmapImage图片</returns>
        public async Task<BitmapImage> GetPageBackground()
        {
            GenerateBgService bgService = new GenerateBgService();
            BingImageData bingImageData = Task.Run(bgService.getBingImageAsync).Result;
            Image item = bingImageData.images[0];
            Windows.Web.Http.HttpClient http = new Windows.Web.Http.HttpClient();

            IBuffer buffer = await http.GetBufferAsync(new Uri("https://www.bing.com" + item.url));

            BitmapImage img = new BitmapImage();

            using (IRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(buffer);
                stream.Seek(0);
                await img.SetSourceAsync(stream);
            }

            return img;
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
