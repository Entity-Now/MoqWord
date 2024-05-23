using MoqWord.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Services
{
    public class CategoryService(ICategoryRepository repository, IWordService wordService) : BaseService<Category>(repository), ICategoryService
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
        /// 选择词库
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public virtual bool SelectCategory(Category c)
        {
            
        }
    }
}
