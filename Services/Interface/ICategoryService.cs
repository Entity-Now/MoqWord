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
        /// 获取今天的单词
        /// </summary>
        /// <returns></returns>
        List<Word> GetWordsToReview();
        /// <summary>
        /// 获取需要复习的单词
        /// </summary>
        /// <returns></returns>
        List<Word> GetNextWordsToReview();
        /// <summary>
        /// 根据评分更新单词
        /// </summary>
        /// <param name="word"></param>
        /// <param name="grade"></param>
        void UpdateWordAfterReview(Word word, int grade);
    }
}
