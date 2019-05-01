using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System.UserProfile;

namespace DailyPoetry.Services
{
    /// <summary>
    /// 改变用户的桌面壁纸
    /// </summary>
    public class WallpaperService
    {
        /// <summary>
        /// 改变桌面壁纸。
        /// </summary>
        /// <param name="localAppDataFileName"></param>
        /// <returns>是否操作成功。</returns>
        public async Task<bool> SetWallpaperAsync(string localAppDataFileName)
        {
            bool success = false;
            if (UserProfilePersonalizationSettings.IsSupported())
            {
                var uri = new Uri("ms-appdata:///local/" + localAppDataFileName);
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
                UserProfilePersonalizationSettings profileSettings = UserProfilePersonalizationSettings.Current;
                success = await profileSettings.TrySetWallpaperImageAsync(file);
            }
            return success;
        }

        /// <summary>
        /// 改变锁屏壁纸。
        /// </summary>
        /// <param name="localAppDataFileName"></param>
        /// <returns>是否操作成功。</returns>
        public async Task<bool> SetLockScreenAsync(string localAppDataFileName)
        {
            bool success = false;
            if (UserProfilePersonalizationSettings.IsSupported())
            {
                var uri = new Uri("ms-appdata:///local/" + localAppDataFileName);
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
                UserProfilePersonalizationSettings profileSettings = UserProfilePersonalizationSettings.Current;
                success = await profileSettings.TrySetLockScreenImageAsync(file);
            }
            return success;
        }

        /// <summary>
        /// 按照指定的文件名更换壁纸
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public async Task<bool> WallpaperChanger()
        {
            string filename = DateTime.Today.ToShortDateString().Replace("/", "") + "Wallpaper.jpg";
            bool isSucceed = await SetWallpaperAsync(filename);
            return isSucceed;
        }

        public async Task<bool> LockScreenChanger()
        {
            string filename = DateTime.Today.ToShortDateString().Replace("/", "") + "Wallpaper.jpg";
            bool isSucceed = await SetLockScreenAsync(filename);
            return isSucceed;
        }

        ///// <summary>
        ///// 随机更换壁纸（重载）
        ///// </summary>
        ///// <returns></returns>
        //public async Task<bool> WallpaperChanger()
        //{
        //    string[] fileList = System.IO.Directory.GetFileSystemEntries("Wallpapers/");
        //    for (int i = 0; i < fileList.Length; ++i)
        //    {
        //        fileList[i] =
        //            fileList[i].Substring(fileList[i].IndexOf("/") + 1, fileList[i].Length - fileList[i].IndexOf("/") - 1);
        //    }
        //    //提高随机数不重复概率的种子生成方法，在New Random(SeedParam)时候保证SeedParam是唯一的
        //    Random random = new Random(int.Parse(DateTime.Now.ToString("HHmmssfff")));
        //    int r = random.Next(0, fileList.Length);
        //    bool isSucceed = await SetWallpaperAsync(fileList[r]);
        //    return isSucceed;
        //}
    }
}
