using AntDesign;
using DynamicData;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MoqWord.Attributes;
using MoqWord.Core;
using MoqWord.Extensions;
using MoqWord.Model.Data;
using MoqWord.ModelView;
using MoqWord.Repository.Interface;
using MoqWord.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using static MoqWord.Utils.NativeMethod;
using KeyboardEventArgs = Microsoft.AspNetCore.Components.Web.KeyboardEventArgs;

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
        private List<string> keys = new List<string>();
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

        private void HotKey_PreviewKeyDown(KeyboardEventArgs e)
        {
            var key = e.Key;
            if (keys.Contains(key))
            {
                return;
            }
            keys.Add(key);
        }

        private void HotKey_PreviewKeyUp(KeyboardEventArgs e, ShortcutKeys shortcutKeys)
        {
            var notShortcutKeys = new List<string>
            {
                "Alt", "Control", "Shift", "Meta", "CapsLock", "NumLock", "ScrollLock",
                "Insert", "Delete", "Backspace", "Tab", "ArrowUp", "ArrowDown", "ArrowLeft", "ArrowRight",
                "PrintScreen", "Pause"
            };
            if (keys.Count <= 0)
            {
                return;
            }
            if (keys.Count == 1 && notShortcutKeys.Any(item => item == keys[0]))
            {
                keys.Clear();
                return;
            }
            shortcutKeys.ShortcutName = string.Join(',', keys.Select(k => KeyMap.getKeyName(k)));
            shortcutKeys.Key = KeyMap.getKeyNumber(keys[keys.Count - 1]);
            shortcutKeys.Modifiers = KeyModifiers.None;
            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                shortcutKeys.Modifiers += 2;
            }

            if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
            {
                shortcutKeys.Modifiers += 4;
            }

            if ((Keyboard.Modifiers & ModifierKeys.Alt) != 0)
            {
                shortcutKeys.Modifiers += 1;
            }
            keys.Clear();
            settingService.ShortcutKeysService.UpdateById(shortcutKeys);
            // 重载热键
            ShortcutKeyHelper.ReRegister();
        }
    }
}
