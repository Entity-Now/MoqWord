using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Services.Interface
{
    public interface ICategoryService : IBaseService<Category>
    {
        /// <summary>
        /// 获取所有词库
        /// </summary>
        /// <returns></returns>
        IEnumerable<CategoryDTO> GetAllCategory();
        /// <summary>
        /// 当前是否选择记忆单词本
        /// </summary>
        /// <returns></returns>
        Category IsSelectCategory();
        /// <summary>
        /// 选择词库
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        bool SelectCategory(Category c);
        /// <summary>
        /// 初始化所有单词
        /// </summary>
        void Initialization(int categoryId, int dailyLimit);
        /// <summary>
        /// 获取当前是背诵的第几天
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        int GetCurrentDay(int categoryId);
        /// <summary>
        /// 将指定的groupNumber分类设置为已经记忆完成
        /// </summary>
        /// <param name="WordGroup"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        int SetGroupState(WordGroup wordGroup, bool state);
        /// <summary>
        /// 获取今天的单词
        /// </summary>
        /// <returns></returns>
        List<Word> GetWordsToReview();
        /// <summary>
        /// 根据指定的GroupNumber获取单词列表
        /// </summary>
        /// <param name="groupNumber">指定的GroupNumber</param>
        /// <returns>对应GroupNumber的单词列表</returns>
        List<Word> GetWordsToReviewByGroupNumber(int groupNumber);
        /// <summary>
        /// 根据CategoryId查找出所有不同的GroupNumber，并以升序排序
        /// </summary>
        /// <param name="categoryId">指定的CategoryId</param>
        /// <returns>升序排序的GroupNumber列表</returns>
        List<WordGroup> GetGroupNumbersByCategoryId(int categoryId);
        /// <summary>
        /// 获取需要复习的单词
        /// </summary>
        /// <returns></returns>
        List<Word> GetNextWordsToReview();
        /// <summary>
        /// 根据评分更新单词
        /// </summary>
        /// <param name="word">需要更新的单词</param>
        /// <param name="rating">单词记忆评分</param>
        void UpdateWordAfterReview(Word word, WordRating rating);
    }
}
