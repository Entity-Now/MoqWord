﻿using System;
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
        ObservableCollection<Word> ToDayWords { get; set; }
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
        void Init();
        /// <summary>
        /// 开始播放单词
        /// </summary>
        void Play();
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
        void Looped();
    }
}
