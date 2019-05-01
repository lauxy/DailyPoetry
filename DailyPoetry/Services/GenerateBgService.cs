using Microsoft.Graphics.Canvas;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Graphics.Canvas.Text;
using Newtonsoft.Json;

namespace DailyPoetry.Services
{
    public class GenerateBgService : IBingImageService
    {

        private StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
        private string prefix = DateTime.Today.ToShortDateString().Replace("/", "");
        /// <summary>
        /// 获取Bing每日图片的Json，反序列化为类。
        /// </summary>
        /// <returns>返回BingImageData类</returns>
        public async Task<BingImageData> getBingImageDataAsync()
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
        public async Task<BitmapImage> GetBitmapImageAsync()
        {
            GenerateBgService bgService = new GenerateBgService();
            BingImageData bingImageData = Task.Run(bgService.getBingImageDataAsync).Result;
            Image item = bingImageData.images[0];
            Windows.Web.Http.HttpClient http = new Windows.Web.Http.HttpClient();

            string uri = "https://www.bing.com" + item.url;
            IBuffer buffer = await http.GetBufferAsync(new Uri(uri));
            
            BitmapImage img = new BitmapImage();

            using (IRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(buffer);
                stream.Seek(0);
                await img.SetSourceAsync(stream);
            }

            // Save the bing image to local folder
            SaveImageToLocalFolder(uri);

            return img;
        }

        /// <summary>
        /// 保存Bing图片到本地文件夹。
        /// </summary>
        /// <param name="uri">文件的Url</param>
        public async void SaveImageToLocalFolder(string uri)
        {
            // 如果文件存在则不再执行下面的保存操作。
            if (!File.Exists(storageFolder.Path + @"\" + prefix + "Background.jpg"))
            {
                HttpClient client = new HttpClient();
                var imgStream = await client.GetStreamAsync(new Uri(uri));
                string imgName = prefix + "Background.jpg";
                var file = await storageFolder.CreateFileAsync(imgName, CreationCollisionOption.ReplaceExisting);
                using (var targetStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (imgStream)
                        await imgStream.CopyToAsync(targetStream.AsStreamForWrite());
                }
            }
        }

        /// <summary>
        /// 将文字（诗句）和图片（配图）合成一张图片，作为壁纸或锁屏。
        /// </summary>
        /// <param name="imageName">原配图的名字</param>
        /// <param name="text">诗句的内容</param>
        /// <returns></returns>
        public async Task CreateBackgroundImageAsync(string text)
        {
            Uri imageuri = new Uri("ms-appdata:///local/"+prefix+"Background.jpg");
            
            StorageFile inputFile = await StorageFile.GetFileFromApplicationUriAsync(imageuri);
            BitmapDecoder imagedecoder;
            using (var imagestream = await inputFile.OpenAsync(FileAccessMode.Read))
            {
                imagedecoder = await BitmapDecoder.CreateAsync(imagestream);
            }
            CanvasDevice device = CanvasDevice.GetSharedDevice();
            CanvasRenderTarget renderTarget =
                new CanvasRenderTarget(device, imagedecoder.PixelWidth, imagedecoder.PixelHeight, 96);
            using (var ds = renderTarget.CreateDrawingSession())
            {
                ds.Clear(Colors.Black);
                CanvasBitmap image = await CanvasBitmap.LoadAsync(device, inputFile.Path, 96);
                ds.DrawImage(image);
                CanvasTextFormat format = new CanvasTextFormat();
                format.FontSize = 48;
                ds.DrawText(text,
                    new System.Numerics.Vector2(imagedecoder.PixelWidth-900, imagedecoder.PixelHeight-300),
                    Colors.White,
                    format);
            }
            string filename = prefix+"Wallpaper.jpg";
           
            var file = await storageFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                await renderTarget.SaveAsync(fileStream, CanvasBitmapFileFormat.Png, 1f);
            }
        }
    }
}
