using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Model.Data
{
    public class WordGroup
    {
        public int CategoryId { get; set; }
        /// <summary>
        /// 哪一天
        /// </summary>
        public int GroupDay { get; set; }
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsGrasp { get; set; }
        /// <summary>
        /// 完成比例
        /// </summary>
        public float GraspRatio { get; set; }
    }
}
