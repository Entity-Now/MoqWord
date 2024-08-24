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
using System.Windows.Input;
using static MoqWord.Core.KeyBoardHook;

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
        KeyDownHandle inputHandle = null;
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

        void InputFocusOn(ShortcutKeys sk)
        {
            inputHandle = (keys) =>
            {
                this.InvokeAsync(() =>
                {
                    sk.Keys = String.Join(',', keys.Select(k => KeyMap.keyMap.ContainsKey(k) ? KeyMap.keyMap[k] : ""));
                    StateHasChanged();
                });
            };
            KeyBoardHook.KeysHandle += inputHandle;
            // 设置钩子
            KeyBoardHook.KeysHandle -= NotifyIconHelper.KeyListens;

        }
        void InputBlurOn(ShortcutKeys sk)
        {
            KeyBoardHook.KeysHandle -= inputHandle;// 设置钩子
            KeyBoardHook.KeysHandle += NotifyIconHelper.KeyListens;
            settingService.ShortcutKeysService.SetColumns(s => new()
            {
                Keys = sk.Keys
            }, s => s.Id == sk.Id);
        }
    }
}
