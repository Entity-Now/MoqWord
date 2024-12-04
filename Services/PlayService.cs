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
        public IBookService BookService { get; set; }
        public ISettingService settingService { get; set; }
        public IWordService wordService { get; set; }
        public IWordLogService wordLogService { get; set; }
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

        private CancellationTokenSource _cancellationTokenSource = default;

        public PlayService(IBookService _BookService, ISettingService _settingService, Scheduler _scheduler, IWordLogService wordLogService, IWordService wordService)
        {
            BookService = _BookService;
            settingService = _settingService;
            this.wordLogService = wordLogService;
            this.wordService = wordService;
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
            _toDayWords.Clear();
            _days.Clear();
            // 根据指定日期来记忆单词，否则获取当天单词
            if (groupNumber is int g)
            {
                _toDayWords.AddRange(BookService.GetWordsToReviewByGroupNumber(g));
            }
            else
            {
                _toDayWords.AddRange(BookService.GetWordsToReview());
            }
            DailyLimit = ToDayWords.Count;
            if (DailyLimit > 0)
            {
                _days.AddRange(BookService.GetGroupNumbersByBookId(ToDayWords[0].BookId));
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
                IsLoopPlay = true;
                Looped();
            }
        }
        public virtual async Task Play()
        {
            if (_cancellationTokenSource.Token.IsCancellationRequested)
            {
                return;
            }
            // 获取音源
            (playSound, secondaryPlaySound) = settingService.getCurrentSound();
            // 播放单词所需的时间
            var s = settingService.First();
            var readTime = CurrentWord.WordName.CalculateReadingTime(s.SpeechSpeed);
            // play
            await playSound.PlayAsync(CurrentWord.WordName, _cancellationTokenSource.Token);
            // play text
            //scheduler.AddTempTask(TimeSpan.FromSeconds(readTime + 0.3), async () =>
            //{
            //    if (_cancellationTokenSource.Token.IsCancellationRequested)
            //    {
            //        return;
            //    }
            //    var tran_s = CurrentWord.Translates[0];
            //    await secondaryPlaySound.PlayAsync(s.SecondSoundName, tran_s.Trans.ReplacePartOfSpeech(), s.SoundVolume, s.SpeechSpeed, _cancellationTokenSource.Token);
            //});
            var tran_s = CurrentWord.Translates[0];
            await secondaryPlaySound.PlayAsync(s.SecondSoundName, tran_s.Trans.ReplacePartOfSpeech(), s.SoundVolume, s.SpeechSpeed, _cancellationTokenSource.Token);
            // add log
            wordLogService.InsertList(new List<WordLog> { new WordLog
            {
                WordId = CurrentWord.Id,
                IsRead = true,
                ElapsedDays = 0,
                ScheduledDays = 0,
                Review = DateTime.Now,
                CreateDT = DateTime.Now,
                Rating = WordRating.Easy,
                State = WordState.Learning,
            }});
            // update read count
            wordService.SetColumns(w => new Word()
            {
                Repetition = w.Repetition + 1,
                LastReview = DateTime.Now
            }, w => w.Id == CurrentWord.Id);
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

        public async Task Looped()
        {
            CancelAndResetToken();
            if (!IsLoopPlay || _cancellationTokenSource.Token.IsCancellationRequested)
            {
                return;
            }
            var s = settingService.First();
            int playCount = 0;
            async Task playSound()
            {
                if (!IsLoopPlay || _cancellationTokenSource.Token.IsCancellationRequested)
                {
                    return;
                }
                // 播放单词
                await Play();
                playCount++;
                if (playCount < (int)s.RepeatCount)
                {
                    await playSound();
                }
                else
                {
                    if (CurrentIndex < ToDayWords.Count - 1)
                    {
                        CurrentIndex++;
                    }
                    else
                    {
                        CurrentIndex = 0;
                    }
                    await Looped();
                }
            }
            // play
            await playSound();
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
