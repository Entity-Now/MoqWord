using MoqWord.Model.Enum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Model.Entity
{
    [SugarTable]
    public class WordLog : BaseEntity
    {
        public int? WordId { get; set; }
        [Navigate(NavigateType.OneToMany, nameof(WordId))]
        public Word? Word { get; set; }
        /// <summary>
        /// 难度等级
        /// </summary>
        public WordRating Rating { get; set; }
        /// <summary>
        /// 下次复习的时间
        /// </summary>
        public int ScheduledDays { get; set; }
        /// <summary>
        /// 自上次复习以来过去的时间
        /// </summary>
        public int ElapsedDays { get; set; }
        /// <summary>
        /// 复习时间
        /// </summary>
        public DateTime Review { get; set; }
        /// <summary>
        /// 当前单词状态
        /// </summary>
        public WordState State { get; set; }
        /// <summary>
        /// 听力模式，反之则是记忆模式
        /// </summary>
        public bool IsRead { get; set; }
    }
}
