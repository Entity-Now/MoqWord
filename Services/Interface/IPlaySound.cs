using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Services.Interface
{
    public interface IPlaySound
    {
        /// <summary>
        /// 语言转文字
        /// </summary>
        /// <param name="word">文本</param>
        void Play(string word);
        /// <summary>
        /// 获取所有可用的讲述人列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<Voice> GetVoice();
    }
}
