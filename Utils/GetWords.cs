using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace MoqWord.Utils
{
    public static class GetWords
    {
        public static string Get()
        {
            var services = ServiceHelper.getService().GetService<SettingService>();
            SendCtrlC();
            Thread.Sleep(services != null ? services[1].SelectionInterval : 400);
            return NativeClipboard.GetText();
        }

        public static void Set(string text)
        {
            var services = ServiceHelper.getService().GetService<SettingService>();
            NativeClipboard.SetText(text);
            Thread.Sleep(services != null ? services[1].SelectionInterval : 400);
            SendCtrlV();
        }

        private static void SendCtrlC()
        {
            const uint keyEventKeyup = 2;
            const uint keyEventKeydown = 0;
            // 抬起所有键盘控制键
            NativeMethod.keybd_event(Keys.ControlKey, 0, keyEventKeyup, 0);
            NativeMethod.keybd_event(KeyInterop.VirtualKeyFromKey(Key.LeftAlt), 0, keyEventKeyup, 0);
            NativeMethod.keybd_event(KeyInterop.VirtualKeyFromKey(Key.RightAlt), 0, keyEventKeyup, 0);
            NativeMethod.keybd_event(Keys.LWin, 0, keyEventKeyup, 0);
            NativeMethod.keybd_event(Keys.RWin, 0, keyEventKeyup, 0);
            NativeMethod.keybd_event(Keys.ShiftKey, 0, keyEventKeyup, 0);
            // 按下 Ctrl + Insert
            NativeMethod.keybd_event(Keys.ControlKey, 0, keyEventKeydown, 0);
            NativeMethod.keybd_event(Keys.Insert, 0, keyEventKeydown, 0);
            // 抬起 Ctrl + Insert
            NativeMethod.keybd_event(Keys.Insert, 0, keyEventKeyup, 0);
            NativeMethod.keybd_event(Keys.ControlKey, 0, keyEventKeyup, 0);
        }

        private static void SendCtrlV()
        {
            const uint keyEventKeyup = 2;
            const uint keyEventKeydown = 0;
            // 抬起所有键盘控制键
            NativeMethod.keybd_event(Keys.ControlKey, 0, keyEventKeyup, 0);
            NativeMethod.keybd_event(KeyInterop.VirtualKeyFromKey(Key.LeftAlt), 0, keyEventKeyup, 0);
            NativeMethod.keybd_event(KeyInterop.VirtualKeyFromKey(Key.RightAlt), 0, keyEventKeyup, 0);
            NativeMethod.keybd_event(Keys.LWin, 0, keyEventKeyup, 0);
            NativeMethod.keybd_event(Keys.RWin, 0, keyEventKeyup, 0);
            NativeMethod.keybd_event(Keys.ShiftKey, 0, keyEventKeyup, 0);
            // 按下 Ctrl + V
            NativeMethod.keybd_event(Keys.ShiftKey, 0, keyEventKeydown, 0);
            NativeMethod.keybd_event(Keys.Insert, 0, keyEventKeydown, 0);
            // 抬起 Ctrl + V
            NativeMethod.keybd_event(Keys.Insert, 0, keyEventKeyup, 0);
            NativeMethod.keybd_event(Keys.ShiftKey, 0, keyEventKeyup, 0);
        }
    }
}
