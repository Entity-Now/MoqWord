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
    using FreeScheduler;
    using MoqWord.Extensions;
    using System.Threading;

    public class PlayService : ReactiveObject, IPlayService
    {
        public ICategoryService categoryService { get; set; }
        public ISettingService settingService { get; set; }
        public IPlaySound playSound { get; set; }
        public IPlaySound secondaryPlaySound { get; set; }
        public Scheduler scheduler { get; set; }

        private SourceList<Word> _toDayWords = new SourceList<Word>();
        private readonly ReadOnlyObservableCollection<Word> _words;
        public ReadOnlyObservableCollection<Word> ToDayWords => _words;
        private SourceList<WordGroup> _days = new SourceList<WordGroup>();
        public readonly ReadOnlyObservableCollection<WordGroup> Days;
        public ReadOnlyObservableCollection<WordGroup> DayList => Days;

        private bool isLoopPlay = false;
        public bool IsLoopPlay
        {
            get => isLoopPlay;
            set
            {
                this.RaiseAndSetIfChanged(ref isLoopPlay, value);
                UpdateWords();
            }
        }
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

        private CancellationTokenSource _cancellationTokenSource;

        public PlayService(ICategoryService _categoryService, ISettingService _settingService, DefaultPlaySound _secondaryPlay, Scheduler _scheduler)
        {
            categoryService = _categoryService;
            settingService = _settingService;
            secondaryPlaySound = _secondaryPlay;
            scheduler = _scheduler;

            _toDayWords.Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _words)
                .Subscribe(x =>
                {
                    //MessageBox.Show("集合被改变");
                });
            _days.Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out Days)
                .Subscribe(d => { });

            Init();
        }

        public void Init(int? groupNumber = null)
        {
            playSound = settingService.getCurrentSound();
            _toDayWords.Clear();
            _days.Clear();
            if (groupNumber is int g)
            {
                _toDayWords.AddRange(categoryService.GetWordsToReviewByGroupNumber(g));
            }
            else
            {
                _toDayWords.AddRange(categoryService.GetWordsToReview());
            }
            DailyLimit = ToDayWords.Count;
            if (DailyLimit > 0)
            {
                _days.AddRange(categoryService.GetGroupNumbersByCategoryId(ToDayWords[0].CategoryId));
            }
            CurrentIndex = 0; // 确保在初始化时将 CurrentIndex 设置为 0
            UpdateWords();    // 确保在初始化时调用 UpdateWords 更新单词
        }

        public virtual void Next()
        {
            CancelAndResetToken();
            if (CurrentIndex < ToDayWords.Count - 1)
            {
                CurrentIndex++;
                Play();
            }
            else
            {
                CurrentIndex = 0;
                Play();
            }
        }
        public virtual void Collapse()
        {
            if (IsLoopPlay)
            {
                Stop();
            }
            else
            {
                Looped();
            }
        }
        public virtual void Play()
        {
            // 播放单词所需的时间
            var s = settingService.First();
            var readTime = CurrentWord.WordName.CalculateReadingTime(s.SpeechSpeed);
            // play
            playSound.PlayAsync(CurrentWord.WordName, _cancellationTokenSource.Token);
            // play text
            scheduler.AddTempTask(TimeSpan.FromSeconds(readTime + 0.3), () =>
            {
                secondaryPlaySound.PlayAsync(CurrentWord.Translation.Split("；")[0], _cancellationTokenSource.Token);
            });
        }

        public virtual void Previous()
        {
            CancelAndResetToken();
            if (CurrentIndex > 0)
            {
                CurrentIndex--;
                Play();
            }
        }

        public virtual void Stop()
        {
            CancelAndResetToken();
            IsLoopPlay = false;
        }

        public void Looped()
        {
            CancelAndResetToken();
            if (!IsLoopPlay || _cancellationTokenSource.Token.IsCancellationRequested)
            {
                return;
            }
            var s = settingService.First();
            // 计算下次播放时间
            var wordNameTime = CurrentWord.WordName.CalculateReadingTime(s.SpeechSpeed);
            var translateTime = CurrentWord.Translation.Split("；")[0].CalculateReadingTime(s.SpeechSpeed);
            var nextTime = wordNameTime + translateTime + 0.3;
            int playCount = 0;
            void playSound()
            {
                if (!IsLoopPlay || _cancellationTokenSource.Token.IsCancellationRequested)
                {
                    return;
                }
                // 播放单词
                Play();
                playCount++;
                if (playCount < (int)s.RepeatCount)
                {
                    scheduler.AddTempTask(TimeSpan.FromSeconds(nextTime), playSound);
                }
                else
                {
                    scheduler.AddTempTask(TimeSpan.FromSeconds(nextTime), () =>
                    {
                        if (CurrentIndex < ToDayWords.Count - 1)
                        {
                            CurrentIndex++;
                        }
                        else
                        {
                            CurrentIndex = 0;
                        }
                        Looped();
                    });
                }
            }
            // play
            playSound();
        }


        private void CancelAndResetToken()
        {
            // 如果_cancellationTokenSource已经被初始化，那么取消并释放它
            _cancellationTokenSource?.Cancel();
            // 创建一个新的CancellationTokenSource实例
            _cancellationTokenSource = new CancellationTokenSource();
        }




        private void UpdateWords()
        {
            CurrentWord = ToDayWords.ElementAtOrDefault(CurrentIndex);
            LastWord = ToDayWords.ElementAtOrDefault(CurrentIndex + 1);
            PreviousWord = ToDayWords.ElementAtOrDefault(CurrentIndex - 1);
        }
    }
}
