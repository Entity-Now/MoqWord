using MoqWord.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Model.Entity
{
    public class Setting : BaseEntity
    {
        /// <summary>
        /// 难度等级
        /// </summary>
        public WordRating Rating { get; set; }
        /// <summary>
        /// 背诵时间间隔
        /// </summary>
        public long TimeInterval { get; set; }
        /// <summary>
        /// 记忆的保留率
        /// </summary>
        public double DesiredRetension { get; set; }
        /// <summary>
        /// 难度
        /// </summary>
        public double Difficulty { get; set; }
        /// <summary>
        /// 已用天数
        /// </summary>
        public int ElapsedDays { get; set; }
    }
}
