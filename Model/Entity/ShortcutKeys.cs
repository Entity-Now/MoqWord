using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MoqWord.Utils.NativeMethod;

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
        /// 按键代码
        /// </summary>
        public int Key { get; set; }
        /// <summary>
        /// 功能案件
        /// </summary>
        public KeyModifiers Modifiers { get; set; }
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
        public string ShortcutName { get; set; }
    }
}
