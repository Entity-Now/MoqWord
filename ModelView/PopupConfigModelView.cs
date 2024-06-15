using Edge_tts_sharp;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MoqWord.ModelView
{
    public class PopupConfigModelView : ReactiveObject
    {
        public IPopupConfigService popupConfigService { get; set; }

        /// <summary>
        /// 单词名字体大小
        /// </summary>
        double _wordNameFontSize;
        public double WordNameFontSize
        {
            get => _wordNameFontSize;
            set
            {
                this.RaiseAndSetIfChanged(ref _wordNameFontSize, value);
            }
        }
        /// <summary>
        /// 译文字体大小
        /// </summary>
        double _translationFontSize;
        public double TranslationFontSize
        {
            get => _translationFontSize;
            set
            {
                this.RaiseAndSetIfChanged(ref _translationFontSize, value);
            }
        }
        /// <summary>
        /// 透明度
        /// </summary>
        double _opacity;
        public double Opacity
        {
            get => _opacity;
            set
            {
                this.RaiseAndSetIfChanged(ref _opacity, value);
            }
        }
        /// <summary>
        /// 字体颜色
        /// </summary>
        string _color;
        public string Color
        {
            get => _color;
            set
            {
                this.RaiseAndSetIfChanged(ref _color, value);
            }
        }
        /// <summary>
        /// 背景颜色
        /// </summary>
        string _background;
        public string Background
        {
            get => _background;
            set
            {
                this.RaiseAndSetIfChanged(ref _background, value);
            }
        }
        /// <summary>
        /// 鼠标是否可以穿透
        /// </summary>
        public bool _isPenetrate;
        public bool IsPenetrate
        {
            get=> _isPenetrate;
            set
            {
                this.RaiseAndSetIfChanged(ref _isPenetrate, value);
            }
        }
        /// <summary>
        /// 窗口是否锁定
        /// </summary>
        public bool _isLock;
        public bool IsLock
        {
            get => _isLock;
            set
            {
                this.RaiseAndSetIfChanged(ref _isLock, value);
            }
        }
        public PopupConfigModelView(IPopupConfigService _popupConfigService)
        {
            popupConfigService = _popupConfigService;

            var popupconfig = popupConfigService.First();
            if (popupconfig is null or default(PopupConfig))
            {
                popupconfig = new PopupConfig()
                {
                    Background = "#bebebe",
                    Color = "#bebebe",
                    IsLock = false,
                    IsPenetrate = false,
                    TranslationFontSize = 14,
                    WordNameFontSize = 16,
                    Opacity = 1
                };
                popupConfigService.InsertOrUpdate(popupconfig);
            }
            this.WordNameFontSize = popupconfig.WordNameFontSize;
            this.TranslationFontSize = popupconfig.TranslationFontSize;
            this.Color = popupconfig.Color;
            this.IsLock = popupconfig.IsLock;
            this.Opacity = popupconfig.Opacity;
            this.Background = popupconfig.Background;
            this.IsPenetrate = popupconfig.IsPenetrate;

            this.WhenAnyValue(x => x.WordNameFontSize, 
                x=> x.TranslationFontSize,
                x=> x.Opacity,
                x=> x.Color,
                x=> x.Background,
                x=> x.IsPenetrate,
                x=> x.IsLock
            )
            .Throttle(TimeSpan.FromMilliseconds(1000))
            .Subscribe(source =>
            {
                popupConfigService.SetColumns(p => new()
                {
                    WordNameFontSize = source.Item1,
                    TranslationFontSize = source.Item2,
                    Opacity = source.Item3,
                    Color = source.Item4,
                    Background = source.Item5,
                    IsPenetrate = source.Item6,
                    IsLock = source.Item7
                }, x => true);
            });
        }
    }
}
