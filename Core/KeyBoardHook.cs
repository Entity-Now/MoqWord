using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoqWord.Core
{
    public static class KeyBoardHook
    {
        public static IntPtr _hookID = IntPtr.Zero;
        /// <summary>
        /// 记录当前按下的按键
        /// </summary>
        public static HashSet<Keys> PressedKeys = new HashSet<Keys>();
        /// <summary>
        /// hook事件
        /// </summary>
        public static LowLevelKeyboardProc _proc = HookCallback;
        /// <summary>
        /// 键盘点击事件
        /// </summary>
        public static event KeyDownHandle? KeysHandle;

        // 设置键盘钩子
        public static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        // 钩子处理程序
        public static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys)vkCode;

                if (wParam == (IntPtr)WM_KEYDOWN)
                {
                    PressedKeys.Add(key);
                    // 出发委托
                    KeysHandle?.Invoke(PressedKeys);
                }
                else if (wParam == (IntPtr)WM_KEYUP)
                {
                    PressedKeys.Remove(key);
                }
            }
           return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        // 导入 Windows API 函数
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        public delegate void KeyDownHandle(HashSet<Keys> keys);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
