using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Model.Entity
{
    [SugarTable]
    public class PopupConfig : BaseEntity
    {
        /// <summary>
        /// 单词名字体大小
        /// </summary>
        public double WordNameFontSize
        {
            get;set;
        }
        /// <summary>
        /// 译文字体大小
        /// </summary>
        public double TranslationFontSize
        {
            get; set;
        }
        /// <summary>
        /// 透明度
        /// </summary>
        public double Opacity
        {
            get; set;
        }
        /// <summary>
        /// 字体颜色
        /// </summary>
        public string Color
        {
            get; set;
        } = string.Empty;
        /// <summary>
        /// 背景颜色
        /// </summary>
        public string Background
        {
            get; set;
        } = string.Empty;
        /// <summary>
        /// 鼠标是否可以穿透
        /// </summary>
        public bool IsPenetrate { get; set; }
        /// <summary>
        /// 窗口是否锁定
        /// </summary>
        public bool IsLock { get; set; }
    }
}
