using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace DailyPoetry
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public SettingsPage()
        {
            this.InitializeComponent();
            // 查询AppDataContainer中保存的关于用户是否允许将每日推荐图片设置为壁纸的偏好设置
            if (localSettings.Values["SetWallpaper"] != null)
            {
                if (localSettings.Values["SetWallpaper"].Equals(true))
                {
                    WallpaperSwitch.IsOn = true;
                    WallpaperSwitch.OffContent = "开";
                }
                else
                {
                    WallpaperSwitch.IsOn = false;
                    WallpaperSwitch.OffContent = "关";
                }
            }
            // 查询AppDataContainer中保存的关于用户是否允许将每日推荐图片设置为锁屏的偏好设置
            if (localSettings.Values["SetLockScreen"] != null)
            {
                if (localSettings.Values["SetLockScreen"].Equals(true))
                {
                    LockScreenSwitch.IsOn = true;
                    LockScreenSwitch.OffContent = "开";
                }
                else
                {
                    LockScreenSwitch.IsOn = false;
                    LockScreenSwitch.OffContent = "关";
                }
            }

            if (localSettings.Values["IsMyCreationPageBgDefault"] != null)
            {
                if (localSettings.Values["IsMyCreationPageBgDefault"].Equals(true))
                {
                    BackgroundSwitch.IsOn = true;
                    BackgroundSwitch.OffContent = "开";
                }
                else
                {
                    BackgroundSwitch.IsOn = false;
                    BackgroundSwitch.OffContent = "关";
                }
            }
        }

        private void ToggleSwitch_OnToggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    toggleSwitch.OffContent = "开";
                    localSettings.Values["SetWallpaper"] = true;
                }
                else
                {
                    toggleSwitch.OffContent = "关";
                    localSettings.Values["SetWallpaper"] = false;
                }
            }
        }

        private void LockScreenSwitch_OnToggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    toggleSwitch.OffContent = "开";
                    localSettings.Values["SetLockScreen"] = true;
                }
                else
                {
                    toggleSwitch.OffContent = "关";
                    localSettings.Values["SetLockScreen"] = false;
                }
            }
        }

        /// <summary>
        /// 应用程序采用浅色主题。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LightColorRadioButton_OnChecked(object sender, RoutedEventArgs e)
        {
            localSettings.Values["ThemeStyle"] = "Light";

        }

        /// <summary>
        /// 应用程序采用深色主题。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DarkColorRadioButton_OnChecked(object sender, RoutedEventArgs e)
        {
            localSettings.Values["ThemeStyle"] = "Dark";
            this.RequestedTheme = ElementTheme.Dark;
        }

        /// <summary>
        /// 应用程序与系统主题保持一致。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SystemColorRadioButton_OnChecked(object sender, RoutedEventArgs e)
        {
            localSettings.Values["ThemeStyle"] = "Default";
        }

        /// <summary>
        /// 选择“我的创作”界面的背景是否采用默认背景。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackgroundSwitch_OnToggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    toggleSwitch.OffContent = "开";
                    localSettings.Values["IsMyCreationPageBgDefault"] = true;
                }
                else
                {
                    toggleSwitch.OffContent = "关";
                    localSettings.Values["IsMyCreationPageBgDefault"] = false;
                }
            }
        }

     
    }
}
