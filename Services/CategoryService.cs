using MediatR;
using MoqWord.Components.Page;
using MoqWord.Model.Entity;
using MoqWord.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Services
{
    public class CategoryService(ICategoryRepository repository, IWordService wordService, ISettingService settingService, IMediator mediator) : BaseService<Category>(repository), ICategoryService
    {
        /// <summary>
        /// 获取所有词库
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<CategoryDTO> GetAllCategory()
        {
            return GetAll()
            .Includes(x => x.Tags)
            .ToList().Select(x => new CategoryDTO
            {
                Id = x.Id,
                Name = x.Name,
                Color = x.Color,
                Count = wordService.Count(sub_x => sub_x.CategoryId == x.Id),
                GraspCount = wordService.Count(sub_x => sub_x.CategoryId == x.Id && sub_x.Grasp)
            });
        }
        /// <summary>
        /// 当前是否选择记忆单词本
        /// </summary>
        /// <returns></returns>
        public virtual Category? IsSelectCategory()
        {
            var settings = settingService.GetAll().Includes(c => c.CurrentCategory).First();
            if (settings.CurrentCategory is not null and not default(Category))
            {
                return settings.CurrentCategory;
            }
            return null;
        }
        /// <summary>
        /// 选择词库
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public virtual bool SelectCategory(Category c)
        {
            var settings = settingService.Get();
            SetColumns(c => new() { IsCurrent = false }, x => x.IsCurrent);
            Initialization(c.Id, settings.EverDayCount);
            settings.CurrentCategoryId = c.Id;
            settings.CurrentCategory = c;
            var result = settingService.Update(settings, s => s.Id == settings.Id) > 0;
            if (result)
            {
                mediator.Publish(new CategoryNotify(c));
            }
            return result;
        }
        /// <summary>
        /// 初始化所有单词
        /// </summary>
        public void Initialization(int categoryId, int dailyLimit)
        {
            var words = wordService.GetAll().ToList();
            var today = DateTime.Today;
            int totalWords = words.Count;
            int daysNeeded = (int)Math.Ceiling((double)totalWords / dailyLimit);

            for (int i = 0; i < totalWords; i++)
            {
                var word = words[i];
                word.EasinessFactor = 2.5;
                word.Interval = 1;
                word.Repetition = 0;
                word.LastReview = today;
                word.Due = today.AddDays((i / dailyLimit) + 1);
                word.Reps = 0;
                word.Grasp = false;

                // 设置单词的分组编号
                word.GroupNumber = (i / dailyLimit) + 1;
            }
            wordService.Update(words, x => true);
            SetColumns(c => new()
            {
                ElapsedDays = 0,
                IsCurrent = true,
                ScheduledDays = daysNeeded
            }, x => x.Id == categoryId);
        }
        /// <summary>
        /// 获取今天的单词
        /// </summary>
        /// <returns></returns>
        public virtual List<Word> GetWordsToReview()
        {
            var settings = settingService.Get();
            var today = DateTime.Today;

            // 筛选出今天或之前需要复习的单词
            var wordsToReview = wordService.GetQuery(w => w.Due <= today && !w.Grasp).OrderBy(w => w.Due).Take(settings.EverDayCount).ToList();

            return wordsToReview;
        }

        /// <summary>
        /// 根据指定的GroupNumber获取单词列表
        /// </summary>
        /// <param name="groupNumber">指定的GroupNumber</param>
        /// <returns>对应GroupNumber的单词列表</returns>
        public virtual List<Word> GetWordsToReviewByGroupNumber(int groupNumber)
        {
            var settings = settingService.Get();

            // 获取指定GroupNumber的单词
            var wordsToReview = wordService.GetQuery(w => w.GroupNumber == groupNumber && !w.Grasp).OrderBy(w => w.Due).Take(settings.EverDayCount).ToList();

            return wordsToReview;
        }


        /// <summary>
        /// 获取需要复习的单词，每次获取的数量由dailyLimit决定
        /// </summary>
        /// <returns></returns>
        public virtual List<Word> GetNextWordsToReview()
        {
            var settings = settingService.Get();
            var today = DateTime.Today;

            // 获取今天或之前需要复习的单词，并按照Due日期排序，排除已掌握的单词和已复习的单词
            var wordsToReview = wordService
                .GetQuery(w => w.Due <= today && w.Grasp)
                .OrderBy(w => w.Due)
                .Take(settings.EverDayCount)
                .ToList();

            return wordsToReview;
        }

        /// <summary>
        /// 根据CategoryId查找出所有不同的GroupNumber，并以升序排序
        /// </summary>
        /// <param name="categoryId">指定的CategoryId</param>
        /// <returns>升序排序的GroupNumber列表</returns>
        public virtual List<int> GetGroupNumbersByCategoryId(int categoryId)
        {
            // 获取指定CategoryId的所有不同的GroupNumber，并升序排序
            var groupNumbers = wordService.GetQuery(w => w.CategoryId == categoryId)
                                           .Select(w => w.GroupNumber)
                                           .Distinct()
                                           .OrderBy(gn => gn)
                                           .ToList();

            return groupNumbers;
        }


        /// <summary>
        /// 根据评分更新单词
        /// </summary>
        /// <param name="word">需要更新的单词</param>
        /// <param name="rating">单词记忆评分</param>
        public virtual void UpdateWordAfterReview(Word word, WordRating rating)
        {
            // 获取当前设置
            var settings = settingService.Get();

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
