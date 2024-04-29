using MoqWord.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Model.Entity
{
    public class WordLog : BaseEntity
    {
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
    }
}
