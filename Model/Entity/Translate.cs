using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Model.Entity
{
    [SugarTable]
    public class Translate : BaseEntity
    {
        public int WordId { get; set; }
        [Navigate(NavigateType.ManyToOne, nameof(WordId))]
        public Word Word { get; set; }
        /// <summary>
        /// 翻译内容
        /// </summary>
        public string Trans { get; set; }
    }
}
