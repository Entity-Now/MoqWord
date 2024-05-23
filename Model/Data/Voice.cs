using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Model.Data
{
    public class Voice
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 简短名称
        /// </summary>
        public string ShortName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public string Locale { get; set; }
        /// <summary>
        /// 建议的编码器
        /// </summary>
        public string SuggestedCodec { get; set; }
    }
}
