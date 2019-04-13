using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        private void ToggleSwitch_OnToggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    toggleSwitch.OffContent = "开";
                }
                else
                {
                    toggleSwitch.OffContent = "关";
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
                }
                else
                {
                    toggleSwitch.OffContent = "关";
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
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 应用程序采用深色主题。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DarkColorRadioButton_OnChecked(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            this.RequestedTheme = ElementTheme.Dark;
        }

        /// <summary>
        /// 应用程序与系统主题保持一致。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SystemColorRadioButton_OnChecked(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
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
                }
                else
                {
                    toggleSwitch.OffContent = "关";
                }
            }
        }

     
    }
}
