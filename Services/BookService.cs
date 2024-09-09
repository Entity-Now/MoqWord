using MediatR;
using MoqWord.Components.Page;
using MoqWord.Model.Entity;
using MoqWord.Services.Interface;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Services
{
    public class BookService(IBookRepository repository, IWordService wordService, ISettingService settingService, IMediator mediator) : BaseService<Book>(repository), IBookService
    {
        /// <summary>
        /// 获取所有词库
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<BookDTO> GetAllBook()
        {
            return All()
            .Includes(x => x.Tags)
            .ToList().Select(x => new BookDTO
            {
                Id = x.Id,
                Name = x.Name,
                Color = x.Color,
                Count = wordService.Count(sub_x => sub_x.BookId == x.Id),
                GraspCount = wordService.Count(sub_x => sub_x.BookId == x.Id && sub_x.Grasp)
            });
        }
        /// <summary>
        /// 当前是否选择记忆单词本
        /// </summary>
        /// <returns></returns>
        public virtual Book? IsSelectBook()
        {
            var settings = settingService.All().Includes(c => c.CurrentBook).First();
            if (settings.CurrentBook is not null and not default(Book))
            {
                return settings.CurrentBook;
            }
            return null;
        }
        /// <summary>
        /// 选择词库
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public virtual bool SelectBook(Book c)
        {
            var settings = settingService.First();
            var Book = First(x => x.Id == c.Id);
            if (!Book.IsCurrent)
            {
                Initialization(c.Id, settings.EverDayCount);
            }
            var result = settingService.SetColumns(s => new()
            {
                CurrentBookId = c.Id,
            }, s => s.Id == settings.Id) > 0;
            if (result)
            {
                mediator.Publish(new BookNotify(c));
            }
            return result;
        }
        /// <summary>
        /// 初始化所有单词
        /// </summary>
        public void Initialization(int BookId, int dailyLimit)
        {
            var words = wordService.Query(c => c.BookId == BookId).ToList();
            var today = DateTime.Today.Date;
            int totalWords = words.Count;
            int daysNeeded = (int)Math.Ceiling((double)totalWords / dailyLimit);

            for (int i = 0; i < totalWords; i++)
            {
                var word = words[i];
                word.EasinessFactor = 2.5;
                word.Interval = 1;
                word.Repetition = 0;
                word.LastReview = today;
                word.Due = today.AddDays((i / dailyLimit));
                word.Reps = 0;
                word.Grasp = false;

                // 设置单词的分组编号
                word.GroupNumber = (i / dailyLimit);
            }
            wordService.Update(words, x => true);
            SetColumns(c => new()
            {
                ElapsedDays = 0,
                IsCurrent = true,
                ScheduledDays = daysNeeded
            }, x => x.Id == BookId);
        }
        /// <summary>
        /// 获取当前背诵到第几天
        /// </summary>
        /// <returns></returns>
        public virtual int GetCurrentDay(int BookId)
        {
            var words = wordService.Query(c => c.BookId == BookId && !c.Grasp).OrderBy(c => c.Due).ToList();
            if (words.FirstOrDefault() is Word w)
            {
                return w.GroupNumber;
            }
            return 1;
        }
        /// <summary>
        /// 将指定的groupNumber分类设置为已经记忆完成
        /// </summary>
        /// <param name="groupNumber"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual int SetGroupState(WordGroup wordGroup, bool state)
        {
            return wordService.SetColumns(w => new()
            {
                Grasp = state
            }, w => w.GroupNumber == wordGroup.GroupDay && w.BookId == wordGroup.BookId);
        }
        /// <summary>
        /// 获取今天的单词
        /// </summary>
        /// <returns></returns>
        public virtual List<Word>? GetWordsToReview()
        {
            var settings = settingService.First();
            if (settings == null || settings.CurrentBookId == null)
            {
                return default(List<Word>);
            }
            var today = DateTime.Today.Date.AddDays(1);

            // 筛选出今天或之前需要复习的单词
            var wordsToReview = wordService.Query(w => /*w.Due <= today && */!w.Grasp && w.BookId == settings.CurrentBookId).OrderBy(w => w.Due).Take(settings.EverDayCount).ToList();

            return wordsToReview;
        }

        /// <summary>
        /// 根据指定的GroupNumber获取单词列表
        /// </summary>
        /// <param name="groupNumber">指定的GroupNumber</param>
        /// <returns>对应GroupNumber的单词列表</returns>
        public virtual List<Word>? GetWordsToReviewByGroupNumber(int groupNumber)
        {
            var settings = settingService.First();
            if (settings == null || settings.CurrentBookId == null)
            {
                return default(List<Word>);
            }
            // 获取指定GroupNumber的单词
            var wordsToReview = wordService.Query(w => w.GroupNumber == groupNumber && !w.Grasp).OrderBy(w => w.Due).Take(settings.EverDayCount).ToList();

            return wordsToReview;
        }


        /// <summary>
        /// 获取需要复习的单词，每次获取的数量由dailyLimit决定
        /// </summary>
        /// <returns></returns>
        public virtual List<Word> GetNextWordsToReview()
        {
            var settings = settingService.First();
            var today = DateTime.Today;

            // 获取今天或之前需要复习的单词，并按照Due日期排序，排除已掌握的单词和已复习的单词
            var wordsToReview = wordService
                .Query(w => w.Due <= today && w.Grasp)
                .OrderBy(w => w.Due)
                .Take(settings.EverDayCount)
                .ToList();

            return wordsToReview;
        }

        /// <summary>
        /// 根据BookId查找出所有不同的GroupNumber，并以升序排序
        /// </summary>
        /// <param name="BookId">指定的BookId</param>
        /// <returns>升序排序的GroupNumber列表</returns>
        public virtual List<WordGroup> GetGroupNumbersByBookId(int BookId)
        {
            // 获取指定BookId的所有不同的GroupNumber，并升序排序
            var wordGroups = wordService.Query(w => w.BookId == BookId)
                            .GroupBy(w => new { w.BookId, w.GroupNumber })
                            .Select(g => new WordGroup
                            {
                                BookId = g.BookId,
                                GroupDay = g.GroupNumber,
                                IsGrasp = SqlFunc.AggregateSum(SqlFunc.IIF(g.Grasp, 1, 0)) == SqlFunc.AggregateCount(g.GroupNumber),
                                GraspRatio = SqlFunc.AggregateSum(SqlFunc.IIF(g.Grasp, 1, 0)) / (float)SqlFunc.AggregateCount(g.GroupNumber)
                            })
                            .OrderBy(g => g.GroupDay)
                            .ToList();



            return wordGroups;
        }


        /// <summary>
        /// 根据评分更新单词
        /// </summary>
        /// <param name="word">需要更新的单词</param>
        /// <param name="rating">单词记忆评分</param>
        public virtual void UpdateWordAfterReview(Word word, WordRating rating)
        {
            // 获取当前设置
            var settings = settingService.First();

            double easinessFactor = word.EasinessFactor ?? 2.5;
            int repetition = word.Repetition ?? 0;
            double interval = word.Interval ?? 1;

            // 将 WordRating 转换为数值
            int grade = (int)rating;

            // 根据评分更新易记因子
            easinessFactor = Math.Max(1.3, easinessFactor + 0.1 - (4 - grade) * (0.08 + (4 - grade) * 0.02) * settings.Difficulty);

            if (grade >= 3)
            {
                if (repetition == 0)
                {
                    interval = 1;
                }
                else if (repetition == 1)
                {
                    interval = 6;
                }
                else
                {
                    interval *= easinessFactor;
                }
                repetition++;
                word.Reps = (word.Reps ?? 0) + 1;
                word.Grasp = true;
                word.ReciteTime = DateTime.Now;
            }
            else
            {
                repetition = 0;
                interval = 1;
                word.Lapses = (word.Lapses ?? 0) + 1;
            }

            // 调整记忆间隔，应用DesiredRetension和TimeInterval
            double retensionInterval = Math.Max(1, interval * settings.TimeInterval * settings.DesiredRetension);

            word.EasinessFactor = easinessFactor;
            word.Repetition = repetition;
            word.Interval = interval;
            word.LastReview = DateTime.Now;
            word.Due = DateTime.Now.AddDays(retensionInterval);
            wordService.Update(word, w => w.Id == word.Id);
        }

    }
}
