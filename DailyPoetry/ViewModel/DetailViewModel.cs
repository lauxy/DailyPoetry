using DailyPoetry.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace DailyPoetry.ViewModel
{
    public class DetailViewModel : ViewModelBase
    {
        private KnowledgeService _knowledgeService;

        private static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        // Xaml-Controls-Gallery/XamlControlsGallery/ControlPages/ComboBoxPage.xaml.cs
        public List<Tuple<string, FontFamily>> Fonts { get; } = new List<Tuple<string, FontFamily>>()
            {
                new Tuple<string, FontFamily>("等线", new FontFamily("DengXian")),
                new Tuple<string, FontFamily>("仿宋", new FontFamily("FangSong")),
                new Tuple<string, FontFamily>("楷体", new FontFamily("KaiTi")),
                new Tuple<string, FontFamily>("微软雅黑", new FontFamily("Microsoft YaHei UI")),
                new Tuple<string, FontFamily>("宋体", new FontFamily("SimSun")),
                new Tuple<string, FontFamily>("黑体", new FontFamily("SimHei")),
            };

        private FontFamily _selectedFont;

        public FontFamily SelectedFont
        {
            get => _selectedFont;
            set => Set(nameof(SelectedFont), ref _selectedFont, value);
        }

        public DetailViewModel(KnowledgeService knowledgeService)
        {
            _knowledgeService = knowledgeService;
            if (localSettings.Values["Font"] != null)
                SelectedFont = Fonts[(int)localSettings.Values["Font"]].Item2;
            else
            {
                SelectedFont = Fonts[3].Item2;
                localSettings.Values["Font"] = 3;
            }
        }

        public void RecordRecentView(int PoetryId)
        {
            using (_knowledgeService.Entry())
            {
                _knowledgeService.AddRecentViewItem(PoetryId);
            }
        }

        public void UpdateFont(int i)
        {
            localSettings.Values["Font"] = i;
        }

        public bool SwitchFavoriteStatus(int PoetryId)
        {
            bool finalFavortiateStatus = false;
            using (_knowledgeService.Entry())
            {
                if (_knowledgeService.PoetryIsLiked(PoetryId))
                {
                    _knowledgeService.DeleteFavoriteItemByPoetryIdItem(PoetryId);
                    finalFavortiateStatus = false;
                }
                else
                {
                    _knowledgeService.AddFavoriteItem(PoetryId);
                    finalFavortiateStatus = true;
                }
            }
            return finalFavortiateStatus;
        }

        public bool GetFavoriteStatus(int PoetryId)
        {
            bool finalFavortiateStatus = false;
            using (_knowledgeService.Entry())
            {
                finalFavortiateStatus = _knowledgeService.PoetryIsLiked(PoetryId);
            }
            return finalFavortiateStatus;
        }
    }
}
