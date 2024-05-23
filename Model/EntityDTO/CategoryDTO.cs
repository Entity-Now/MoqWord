using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Model.EntityDTO
{
    public class CategoryDTO : BaseEntity
    {
        /// <summary>
        /// 分类名
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 分类描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsFinish { get; set; }
        /// <summary>
        /// 是否当前背诵的分类
        /// </summary>
        public bool IsCurrent { get; set; }
        /// <summary>
        /// 已用天数
        /// </summary>
        public int ElapsedDays { get; set; }
        /// <summary>
        /// 计划的天数
        /// </summary>
        public int ScheduledDays { get; set; }
        /// <summary>
        /// 分类颜色
        /// </summary>
        public string Color { get; set; }
        public int Count { get; set; }
        public int GraspCount { get; set; }
        public int Precentage { get; set; }
        public List<Tag> Tags { get; set; }
        /// <summary>
        /// 此分类下的列表
        /// </summary>
        public List<Word> Words { get; set; }
    }
}
