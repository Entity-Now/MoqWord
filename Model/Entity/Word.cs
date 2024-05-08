using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Model.Entity
{
    [SugarTable]
    public class Word : BaseEntity
    {
        /// <summary>
        /// 单词名
        /// </summary>
        public string WordName { get; set; }
        /// <summary>
        /// 音标注解
        /// </summary>
        public string Annotation { get; set; }
        /// <summary>
        /// 译文2
        /// </summary>
        public string Translation { get; set; }
        /// <summary>
        /// 单词的定义
        /// </summary>
        public string Definition { get; set; }
        /// <summary>
        /// 单词的词性
        /// </summary>
        public string PartOfSpeech { get; set; }
        /// <summary>
        /// 下次背诵间隔
        /// </summary>
        public double Interval { get; set; }
        /// <summary>
        /// 下一次复习的时间
        /// </summary>
        public DateTime Due { get; set; }
        /// <summary>
        /// 背诵次数
        /// </summary>
        public int Repetition { get; set; }
        /// <summary>
        /// 失误次数或者遗忘次数
        /// </summary>
        public int Lapses { get; set; }
        /// <summary>
        /// 已经完成的次数
        /// </summary>
        public int Reps { get; set; }
        /// <summary>
        /// 记忆难度因子
        /// </summary>
        public double EasinessFactor { get; set; }
        /// <summary>
        /// 是否已经掌握
        /// </summary>
        public bool Grasp { get; set; }
        /// <summary>
        /// 上次复习时间
        /// </summary>
        public DateTime LastReview { get; set; }
        /// <summary>
        /// 掌握时间
        /// </summary>
        public DateTime ReciteTime { get; set; }
        public int CategoryId { get; set; }
        [Navigate(NavigateType.ManyToOne, nameof(CategoryId))]
        public Category Category { get; set; }
    }
}
