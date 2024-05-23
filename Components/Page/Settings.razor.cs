using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MoqWord.Attributes;
using MoqWord.Extensions;
using MoqWord.Model.Data;
using MoqWord.Repository.Interface;
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
        protected ISettingService service { get; set; }
        public List<SettingItem> SourceList { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SourceList = service.GetAllSettings(() => message.Error("修改失败~")).ToList();
        }
        private void InputHandle<T>(T curr, SettingItem item)
        {
            item.Handle(curr);
            item.Value = curr;
        }
    }
}
