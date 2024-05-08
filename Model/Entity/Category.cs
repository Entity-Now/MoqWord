using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Model.Entity
{
    [SugarTable]
    public class Category : BaseEntity
    {
        /// <summary>
        /// 分类名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 分类描述
        /// </summary>
        public string Nescription { get; set; }
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
        /// 此分类下的列表
        /// </summary>
        [Navigate(NavigateType.OneToMany, nameof(Word.CategoryId))]
        public List<Word> Words { get; set; }
    }
}
