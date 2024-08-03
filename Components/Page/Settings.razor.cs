using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MoqWord.Attributes;
using MoqWord.Core;
using MoqWord.Extensions;
using MoqWord.Model.Data;
using MoqWord.ModelView;
using MoqWord.Repository.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoqWord.Components.Page
{
    public partial class Settings
    {
        [Inject]
        protected IMessageService message { get; set; }
        [Inject]
        protected SettingModelView settingService
        {
            get => ViewModel;
            set => ViewModel = value;
        }

        private string currentTag = "default";
        public List<SettingItem> SourceList { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            
            SourceList = ViewModel.SettingService.GetAllSettings(() => message.Error("修改失败~")).ToList();
        }
        private void InputHandle<T>(T curr, SettingItem item)
        {
            item.Handle(curr);
            item.Value = curr;
        }

        private void installShowDeskTop()
        {
            KeyBoardHook.KeysHandle += (kes) =>
            {

            };
        }
        private void KeyboardHandle(HashSet<Keys> keys)
        {

        }
    }
}
