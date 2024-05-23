using MoqWord.Attributes;
using MoqWord.Model.Enum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Model.Entity
{
    [SugarTable]
    public class Setting : BaseEntity
    {
        /// <summary>
        /// 暗色主题
        /// </summary>
        [CellType("暗色主题", false, CellType.Switch)]
        public bool DarkNess { get; set; }
        /// <summary>
        /// 使用音效
        /// </summary>
        [CellType("使用音效", false, CellType.Switch)]
        public bool UsingSound { get; set; }
        /// <summary>
        /// 每日记忆数量
        /// </summary>
        [CellType("每日记忆数量", 20, CellType.TextBox, typeof(int))]
        public int EverDayCount { get; set; }
        /// <summary>
        /// 单词播放次数
        /// </summary>
        [CellType("单词播放次数", RepeatType.Three, CellType.ComboBox, typeof(RepeatType))]
        public RepeatType RepeatCount { get; set; }
        /// <summary>
        /// 背诵时间间隔
        /// </summary>
        [CellType("背诵时间间隔", 365, CellType.TextBox, typeof(long))]
        public long TimeInterval { get; set; }
        /// <summary>
        /// 记忆的保留率
        /// </summary>
        [CellType("记忆的保留率", 0.9, CellType.TextBox, typeof(double))]
        public double DesiredRetension { get; set; }
        /// <summary>
        /// 难度
        /// </summary>
        [CellType("难度", 0.5, CellType.TextBox, typeof(double))]
        public double Difficulty { get; set; }
        /// <summary>
        /// 音源
        /// </summary>
        [CellType("音源", Sound.Default, CellType.ComboBox, typeof(Sound))]
        public Sound SoundSource { get; set; }
        /// <summary>
        /// 音频名
        /// </summary>
        [CellType("音频名", "未定义", CellType.TextBox, typeof(string))]
        public string SoundName { get; set; }
        /// <summary>
        /// 编码译码器
        /// </summary>
        [CellType("编码译码器", null, CellType.TextBox, typeof(string), Visible = false)]
        public string? SuggestedCodec { get; set; }
        /// <summary>
        /// 音频播放速度
        /// </summary>
        [CellType(Title = "音频播放速度", Value = 0, CellType = CellType.Slider, ValueType = typeof(double), MinValue = -100.0, MaxValue = 100.0)]
        public double SpeechSpeed { get; set; }
        /// <summary>
        /// 音频音量
        /// </summary>
        [CellType(Title = "音频音量", Value = 0, CellType = CellType.Slider, ValueType = typeof(double), MinValue = 0.0, MaxValue = 100.0)]
        public double SoundVolume { get; set; }
        /// <summary>
        /// 是否开机自启
        /// </summary>
        [CellType("是否开机自启", false, CellType.Switch)]
        public bool StartWithWindows { get; set; }
    }
}
