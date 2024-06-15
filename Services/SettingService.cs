using Microsoft.Extensions.DependencyInjection;
using MoqWord.Attributes;
using MoqWord.Extensions;
using MoqWord.Model.Enum;
using MoqWord.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MoqWord.Services
{
    public class SettingService(ISettingRepository repository) : BaseService<Setting>(repository), ISettingService
    {
        /// <summary>
        /// 获取当前的音源
        /// </summary>
        /// <returns></returns>
        public virtual IPlaySound getCurrentSound() 
        {
            Dictionary<Sound, Type> SoundMap = new Dictionary<Sound, Type> 
            {
                [Sound.Default] = typeof(DefaultPlaySound),
                [Sound.Youdao] = typeof(YoudaoPlaySound),
                [Sound.Edge] = typeof(EdgePlaySound)
            };
            var setting = repository.First();
            var service = ServiceHelper.getService().GetService(SoundMap[setting.SoundSource]) as IPlaySound;
            return service;
        }
        /// <summary>
        /// 设置当前的音源
        /// </summary>
        /// <returns></returns>
        public virtual bool SetCurrentSound(Sound sound, string name)
        {
            return repository.Update(x => new()
                {
                    SoundSource = sound,
                    SoundName = name
                }, 
                s => s.Id >= 0
            );
        }
        /// <summary>
        /// 设置重复播放次数
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public virtual bool SetRepeatCount(RepeatType count)
        {
            return repository.Update(x => new()
            {
                RepeatCount = count
            },
                s => s.Id >= 0
            );
        }
        /// <summary>
        /// 设置主题
        /// </summary>
        /// <param name="DarkNess"></param>
        /// <returns></returns>
        public virtual bool SetTheme(bool DarkNess)
        {
            return repository.Update(x => new()
            {
                DarkNess = DarkNess
            },
                s => s.Id >= 0
            );
        }
        /// <summary>
        /// 获取setting列表
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<SettingItem> GetAllSettings(Action errorBack)
        {
            var setting = repository.First();
            return typeof(Setting)
                .GetTypeInfo()
                .GetProperties()
                .Where(x => x != null && x.GetCustomAttributes<CellTypeAttribute>().Any())
                .Where(x => x != null && x.GetCustomAttribute<CellTypeAttribute>() is { Visible: true })
                .Select(x => new SettingItem
                {
                    PropertyName = x.Name,
                    Value = setting.GetValue(x.Name),
                    Cell = x.GetCustomAttribute<CellTypeAttribute>(),
                    Handle = (value) =>
                    {
                        setting.SetValue(x.Name, value);
                        var result = repository.UpdateById(setting);
                        if (result <= 0)
                        {
                            errorBack();
                        }
                    }
                });
        }
    }
}
