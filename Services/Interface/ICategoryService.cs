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
        /// 选择词库
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        bool SelectCategory(Category c);
    }
}
