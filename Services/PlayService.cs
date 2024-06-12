using MoqWord.Services.Interface;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Services
{

    public class PlayService : ReactiveObject, IPlayService
    {
        public ICategoryService categoryService { get; set; }
        public ISettingService settingService { get; set; }
        public IPlaySound playSound { get; set; }
        private ObservableCollection<Word> _toDayWords;
        public ObservableCollection<Word> ToDayWords
        {
            get => _toDayWords;
            set => this.RaiseAndSetIfChanged(ref _toDayWords, value);
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

        public PlayService(ICategoryService _categoryService, ISettingService _settingService)
        {
            categoryService = _categoryService;
            settingService = _settingService;
            Init();
        }

        public void Init()
        {
            playSound = settingService.getCurrentSound();
            ToDayWords = new ObservableCollection<Word>(categoryService.GetWordsToReview());
            DailyLimit = ToDayWords.Count;
            UpdateWords();
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
