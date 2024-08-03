using DynamicData;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.ModelView
{
    public class SettingModelView : ReactiveObject
    {
        public ISettingService SettingService { get; set; }
        public IShortcutKeysService ShortcutKeysService { get; set; }

        Sound _sound;
        public Sound Sound
        {
            get => _sound;
            set
            {
                this.RaiseAndSetIfChanged(ref _sound, value);
            }
        }

        string soundName = "";
        public string SoundName
        {
            get => soundName;
            set
            {
                this.RaiseAndSetIfChanged(ref soundName, value);
            }
        }

        double speechSpeed;
        public double SpeechSpeed
        {
            get => speechSpeed;
            set
            {
                this.RaiseAndSetIfChanged(ref speechSpeed, value);
            }
        }

        double soundVolume;
        public double SoundVolume
        {
            get => soundVolume;
            set
            {
                this.RaiseAndSetIfChanged(ref soundVolume, value);
            }
        }
        /// <summary>
        /// 音频列表
        /// </summary>
        SourceList<Voice> soundList = new SourceList<Voice>();
        private readonly ReadOnlyObservableCollection<Voice> _soundList;
        public ReadOnlyObservableCollection<Voice> SoundList => _soundList;
        /// <summary>
        /// 
        /// </summary>
        SourceList<ShortcutKeys> shortcutKeys = new SourceList<ShortcutKeys>();
        public IObservable<IChangeSet<ShortcutKeys>> ShortcutKeys => shortcutKeys.Connect();

        public SettingModelView(ISettingService settingService, IShortcutKeysService _shortcutKeysService)
        {
            SettingService = settingService;
            ShortcutKeysService = _shortcutKeysService;
            var s = settingService.First();
            Sound = s.SoundSource;
            SoundName = s.SoundName;
            SoundVolume = s.SoundVolume;
            SpeechSpeed = s.SpeechSpeed;
            soundList.Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _soundList)
                .Subscribe();

            this.WhenAnyValue(v => v.Sound)
                .Subscribe(r =>
                {
                    var voices = settingService.getSound(r);
                    soundList.Clear();
                    soundList.AddRange(voices.GetVoice());
                });
            this.WhenAnyValue(v => v.Sound, v => v.SoundName, v => v.SpeechSpeed, v => v.SoundVolume)
                .Throttle(TimeSpan.FromMilliseconds(1000))
                .Subscribe(r =>
                {
                    SettingService.SetColumns(s => new Setting
                    {
                        SoundSource  = r.Item1,
                        SoundName = r.Item2,
                        SpeechSpeed = r.Item3,
                        SoundVolume = r.Item4
                    }, x => x.Id > 0);
                });

            // 加载快捷键列表
            shortcutKeys.AddRange(ShortcutKeysService.All().ToArray());
        }
    }
}
