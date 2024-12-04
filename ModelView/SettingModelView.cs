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

        string soundName = "";
        public string SoundName
        {
            get => soundName;
            set
            {
                this.RaiseAndSetIfChanged(ref soundName, value);
            }
        }
        Sound _sound;
        public Sound Sound
        {
            get => _sound;
            set
            {
                this.RaiseAndSetIfChanged(ref _sound, value);
            }
        }

        string secondSoundName = "";
        public string SecondSoundName
        {
            get => secondSoundName;
            set
            {
                this.RaiseAndSetIfChanged(ref secondSoundName, value);
            }
        }
        Sound _secondSound;
        public Sound SecondSound
        {
            get => _secondSound;
            set
            {
                this.RaiseAndSetIfChanged(ref _secondSound, value);
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
        /// 音频列表
        /// </summary>
        SourceList<Voice> secondSoundList = new SourceList<Voice>();
        private readonly ReadOnlyObservableCollection<Voice> _secondSoundList;
        public ReadOnlyObservableCollection<Voice>SecondSoundList => _secondSoundList;
        /// <summary>
        /// 
        /// </summary>
        SourceList<ShortcutKeys> shortcutKeys = new SourceList<ShortcutKeys>();
        public ReadOnlyObservableCollection<ShortcutKeys> _shortcutKeys;
        public ReadOnlyObservableCollection<ShortcutKeys> ShortcutKeys => _shortcutKeys;

        public SettingModelView(ISettingService settingService, IShortcutKeysService _shortcutKeysService)
        {
            SettingService = settingService;
            ShortcutKeysService = _shortcutKeysService;
            var s = settingService.First();
            Sound = s.SoundSource;
            SoundName = s.SoundName;
            SecondSound = s.SecondSoundSource;
            SecondSoundName = s.SecondSoundName;
            SoundVolume = s.SoundVolume;
            SpeechSpeed = s.SpeechSpeed;
            soundList.Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _soundList)
                .Subscribe();
            secondSoundList.Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _secondSoundList)
                .Subscribe();

            shortcutKeys.Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _shortcutKeys)
                .Subscribe();
            this.WhenAnyValue(v => v.Sound)
                .Subscribe(r =>
                {
                    var voices = settingService.getSound(r);
                    soundList.Clear();
                    soundList.AddRange(voices.GetVoice());
                });
            this.WhenAnyValue(it => it.SecondSound)
                .Subscribe(r => {
                    var voices = settingService.getSound(r);
                    secondSoundList.Clear();
                    secondSoundList.AddRange(voices.GetVoice());
                });
            this.WhenAnyValue(v => v.Sound, v => v.SoundName, v => v.SecondSound, v => v.SecondSoundName, v => v.SpeechSpeed, v => v.SoundVolume)
                .Throttle(TimeSpan.FromMilliseconds(1000))
                .Subscribe(r =>
                {
                    SettingService.SetColumns(s => new Setting
                    {
                        SoundSource  = r.Item1,
                        SoundName = r.Item2,
                        SecondSoundSource = r.Item3,
                        SecondSoundName = r.Item4,
                        SpeechSpeed = r.Item5,
                        SoundVolume = r.Item6
                    }, x => x.Id > 0);
                });

            // 加载快捷键列表
            shortcutKeys.AddRange(ShortcutKeysService.All().ToArray());
        }
    }
}
