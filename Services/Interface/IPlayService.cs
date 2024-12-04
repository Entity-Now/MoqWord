using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Services.Interface
{
    public interface IPlayService
    {
        /// <summary>
        /// 可以是背诵/复习的单词
        /// </summary>
        ReadOnlyObservableCollection<Word> ToDayWords { get; }
        ReadOnlyObservableCollection<WordGroup> DayList { get; }
        /// <summary>
        /// 是否循环播放中
        /// </summary>
        public bool IsLoopPlay { get; set; }
        /// <summary>
        /// 当前单词的索引
        /// </summary>
        int CurrentIndex { get; set; }
        /// <summary>
        /// 今日背诵数量
        /// </summary>
        int DailyLimit { get; set; }
        /// <summary>
        /// 上一个单词
        /// </summary>
        Word PreviousWord { get; }
        /// <summary>
        /// 下一个单词
        /// </summary>
        Word LastWord { get; }
        /// <summary>
        /// 当前单词
        /// </summary>
        Word CurrentWord { get;}
        /// <summary>
        /// 初始化
        /// </summary>
        void Init(int? groupNumber = null);
        /// <summary>
        /// 切换
        /// </summary>
        void Collapse();
        /// <summary>
        /// 开始播放单词
        /// </summary>
        Task Play();
        /// <summary>
        /// 停止播放单词
        /// </summary>
        void Stop();
        /// <summary>
        /// 下一个单词
        /// </summary>
        void Next();
        /// <summary>
        /// 上一个单词
        /// </summary>
        void Previous();
        /// <summary>
        /// 循环播放
        /// </summary>
        Task Looped();
    }
}
