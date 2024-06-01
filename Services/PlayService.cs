using MoqWord.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Services
{
    public class PlayService : IPlayService
    {
        public ICategoryService categoryService { get; set; }
        public ISettingService settingService { get; set; }
        public IPlaySound playSound { get; set; }
        public List<Word> ToDayWords { get; set; }
        public int CurrentIndex { get; set; } = 0;
        public int DailyLimit { get; set; }


        public PlayService(ICategoryService _categoryService, ISettingService _settingService)
        {
            categoryService = _categoryService;
            settingService = _settingService;
            playSound = settingService.getCurrentSound();
            ToDayWords = categoryService.GetWordsToReview();
            DailyLimit = ToDayWords.Count;
        }
        public virtual void Next()
        {
            CurrentIndex++;
            Play();
        }

        public virtual void Play()
        {
            var word = ToDayWords.ElementAt(CurrentIndex);
            playSound.Play(word.WordName);
        }

        public virtual void Previous()
        {
            CurrentIndex--;
            Play();
        }

        public virtual void Stop()
        {
            throw new NotImplementedException();
        }

        public void Looped()
        {
            throw new NotImplementedException();
        }
    }
}
