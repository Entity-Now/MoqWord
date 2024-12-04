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
        Task PlayAsync(string word, CancellationToken cancelToken = default);
        /// <summary>
        /// 播放音频，并指定音源
        /// </summary>
        /// <param name="soundName"></param>
        /// <param name="word"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        Task PlayAsync(string soundName, string word, double volume, double speed, CancellationToken cancelToken = default);
        /// <summary>
        /// 获取所有可用的讲述人列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<Voice> GetVoice();
    }
}
