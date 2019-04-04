using Windows.System.UserProfile;
using System.Threading.Tasks;
using Windows.Storage;
using System;

namespace DailyPoetry
{
    public class ChangeWallpaper
    {
        // Pass in a relative path to a file inside the local appdata folder 
        public async Task<bool> SetWallpaperAsync(string localAppDataFileName)
        {
            bool success = false;
            if (UserProfilePersonalizationSettings.IsSupported())
            {
                var uri = new Uri("ms-appx:///Wallpapers/" + localAppDataFileName);
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
                UserProfilePersonalizationSettings profileSettings = UserProfilePersonalizationSettings.Current;
                success = await profileSettings.TrySetWallpaperImageAsync(file);
            }
            return success;
        }
    }
}
