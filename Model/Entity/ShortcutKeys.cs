using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Model.Entity
{
    [SugarTable]
    public class ShortcutKeys : BaseEntity
    {
        /// <summary>
        /// 快捷名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 按键列表
        /// </summary>
        public string Keys { get; set; }
        /// <summary>
        /// 调用的方法名
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? Interface { get; set; } = null;
        /// <summary>
        /// 调用的方法名
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string? Method { get; set; } = null;
        /// <summary>
        /// 快捷名
        /// </summary>
        public ShortcutName ShortcutName { get; set; }
    }
}
