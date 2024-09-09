using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Model.Entity
{
    [SugarTable]
    public class Book : BaseEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public string Category { get; set; }
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
        /// <summary>
        /// 是否外部字典
        /// </summary>
        public bool IsExternal { get; set; } = false;
        /// <summary>
        /// 语言
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// 语言分类
        /// </summary>
        public string LanguageCategory { get; set; }
        [Navigate(typeof(BookTag), nameof(BookTag.BookId), nameof(BookTag.TagId))]
        public List<Tag> Tags { get; set; }
        /// <summary>
        /// 此分类下的列表
        /// </summary>
        [Navigate(NavigateType.OneToMany, nameof(Word.BookId))]
        public List<Word> Words { get; set; }
    }
}