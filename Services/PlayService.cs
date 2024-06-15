using DynamicData;
using MoqWord.Services.Interface;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MoqWord.Services
{

    using DynamicData;
    using ReactiveUI;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Windows;

    public class PlayService : ReactiveObject, IPlayService
    {
        public ICategoryService categoryService { get; set; }
        public ISettingService settingService { get; set; }
        public IPlaySound playSound { get; set; }

        private SourceList<Word> _toDayWords = new SourceList<Word>();
        private readonly ReadOnlyObservableCollection<Word> _words;
        public ReadOnlyObservableCollection<Word> ToDayWords => _words;

        private int _currentIndex = 0;
        public int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref _currentIndex, value);
                UpdateWords();
            }
        }

        private int _dailyLimit;
        public int DailyLimit
        {
            get => _dailyLimit;
            set => this.RaiseAndSetIfChanged(ref _dailyLimit, value);
        }

        private Word _currentWord;
        public Word CurrentWord
        {
            get => _currentWord;
            private set => this.RaiseAndSetIfChanged(ref _currentWord, value);
        }

        private Word _lastWord;
        public Word LastWord
        {
            get => _lastWord;
            private set => this.RaiseAndSetIfChanged(ref _lastWord, value);
        }

        private Word _previousWord;
        public Word PreviousWord
        {
            get => _previousWord;
            private set => this.RaiseAndSetIfChanged(ref _previousWord, value);
        }

        public PlayService(ICategoryService _categoryService, ISettingService _settingService)
        {
            categoryService = _categoryService;
            settingService = _settingService;
            _toDayWords.Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _words)
                .Subscribe(x =>
                {
                    //MessageBox.Show("集合被改变");
                });
            Init();
        }

        public void Init()
        {
            playSound = settingService.getCurrentSound();
            _toDayWords.Clear();
            _toDayWords.AddRange(categoryService.GetWordsToReview());

            DailyLimit = ToDayWords.Count;
            CurrentIndex = 0; // 确保在初始化时将 CurrentIndex 设置为 0
            UpdateWords();    // 确保在初始化时调用 UpdateWords 更新单词
        }

        public virtual void Next()
        {
            if (CurrentIndex < ToDayWords.Count - 1)
            {
                CurrentIndex++;
                Play();
            }
        }

        public virtual void Play()
        {
            playSound.Play(CurrentWord.WordName);
        }

        public virtual void Previous()
        {
            if (CurrentIndex > 0)
            {
                CurrentIndex--;
                Play();
            }
        }

        public virtual void Stop()
        {
            throw new NotImplementedException();
        }

        public void Looped()
        {
            throw new NotImplementedException();
        }

        private void UpdateWords()
        {
            CurrentWord = ToDayWords.ElementAtOrDefault(CurrentIndex);
            LastWord = ToDayWords.ElementAtOrDefault(CurrentIndex + 1);
            PreviousWord = ToDayWords.ElementAtOrDefault(CurrentIndex - 1);
        }
    }
}
