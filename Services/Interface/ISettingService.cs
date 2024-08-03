using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Services.Interface
{
    public interface ISettingService : IBaseService<Setting>
    {
        /// <summary>
        /// 获取指定音源
        /// </summary>
        /// <param name="sound"></param>
        /// <returns></returns>
        IPlaySound getSound(Sound sound);
        /// <summary>
        /// 获取当前的音源
        /// </summary>
        /// <returns></returns>
        IPlaySound getCurrentSound();
        /// <summary>
        /// 设置当前的音源
        /// </summary>
        /// <returns></returns>
        bool SetCurrentSound(Sound sound, string name);
        /// <summary>
        /// 设置重复播放次数
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        bool SetRepeatCount(RepeatType count);
        /// <summary>
        /// 设置主题
        /// </summary>
        /// <param name="DarkNess"></param>
        /// <returns></returns>
        bool SetTheme(bool DarkNess);
        /// <summary>
        /// 获取setting列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<SettingItem> GetAllSettings(Action errBack);
    }
}
