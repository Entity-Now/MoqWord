using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MoqWord.Attributes;
using MoqWord.Extensions;
using MoqWord.Model.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Components.Page
{
    public partial class Settings
    {
        [Inject]
        protected IMessageService message { get; set; }
        [Inject]
        protected ISettingRepository settingRepository { get; set; }
        public Setting? Model { get; set; } 
        public List<SettingItem> SourceList { get; set; }
        public Settings()
        {
            settingRepository = ServiceHelper.Services.GetService<ISettingRepository>();
            // get data
            Model = settingRepository.Get();
            if (Model == null)
            {
                Model = new Setting
                {
                    DarkNess = false,
                    UsingSound = false,
                    EverDayCount = 20,
                    RepeatCount = RepeatType.Three,
                    TimeInterval = 365,
                    DesiredRetension = 0.9,
                    Difficulty = 0.5,
                    SoundName = "",
                    SoundSource = Sound.Default,
                    SpeechSpeed = 0,
                    StartWithWindows = false
                };
                settingRepository.InsertOrUpdate(Model);
            }
            SourceList = getList();
        }
        
        private List<SettingItem> getList()
        {
            var list = typeof(Setting)
                .GetTypeInfo()
            .GetProperties()
                .Where(x => x != null && x.GetCustomAttributes<CellTypeAttribute>().Any())
                .Select(x => new SettingItem {
                    PropertyName = x.Name,
                    Value = Model.GetValue(x.Name),
                    Cell = x.GetCustomAttribute<CellTypeAttribute>(),
                    Handle = (value) =>
                    {
                        Model.SetValue(x.Name, value);
                        var result = settingRepository.UpdateById(Model);
                        if (result > 0)
                        {
                            message.Success("修改成功");
                        }
                        else
                        {
                            message.Error("修改失败");
                        }
                    }
                });

            return list.ToList();
        }
    }
}
