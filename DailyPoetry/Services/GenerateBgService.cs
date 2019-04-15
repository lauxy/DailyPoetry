using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml.Media.Imaging;

namespace DailyPoetry.Services
{
    public class GenerateBgService
    {
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
