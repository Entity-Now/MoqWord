using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Model.Entity
{
    [SugarTable]
    public class Personal : BaseEntity
    {
        /// <summary>
        /// 已用天数
        /// </summary>
        public int ElapsedDays { get; set; }
    }
}
