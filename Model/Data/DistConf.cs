using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Model.Data
{
    /// <summary>
    /// 字典配置
    /// </summary>
    public class DistConf
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public string[] tags { get; set; }
        public string url { get; set; }
        public int length { get; set; }
        public string language { get; set; }
        public string languageCategory { get; set; }
    }

}
