using AntDesign;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MoqWord.WpfComponents.Page;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Media;
using Color = System.Windows.Media.Color;
using Icon = System.Drawing.Icon;
using MenuItem = System.Windows.Controls.MenuItem;
using Flection_Sharp;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using MoqWord.Utils;

namespace MoqWord.Helpers
{
    public static class NotifyIconHelper
    {
        public static Window DeskTopNotify = new DeskTop();
        public static HoverTranslate HoverTranslate = new HoverTranslate();
        public static TaskbarIcon tbi { get; set; }
        public static void Icon()
        {
            var show = ReactiveCommand.Create(() =>
            {
                var window = ServiceHelper.Services.GetService<MainWindow>();
                window.Show();
                window.WindowState = WindowState.Normal;
                window.Topmost = true;
                window.Topmost = false;
            });
            var resource = ResourceHelper.GetEmbeddedResourceStream(Constants.Icon);
            tbi = new TaskbarIcon();
            tbi.Icon = new Icon(resource);
            tbi.ToolTipText = Constants.Title;
            var contextMenu = new ContextMenu()
            {
                ItemsSource = new List<MenuItem>()
                {
                    new MenuItem()
                    {
                        Header = "打开程序",
                        Command = show
                    },
                    new MenuItem()
                    {
                        Header = "退出程序",
                        Command = ReactiveCommand.Create(() =>
                        {
                            WindowHelper.Close();
                        })
                    }
                }
            };
            tbi.DoubleClickCommand = show;
            tbi.ContextMenu = contextMenu;
            
        }

        public static void Show()
        {
            if (DeskTopNotify.Visibility == Visibility.Visible)
            {
                DeskTopNotify.Hide();
                return;
            }
            DeskTopNotify.Show();
        }
        public static void ShowHoverTranslate()
        {
            if (HoverTranslate.Visibility == Visibility.Visible)
            {
                HoverTranslate.Hide();
                return;
            }

            // 显示窗口
            HoverTranslate.CurrentString = GetWords.Get();
            HoverTranslate.WindowStartupLocation = WindowStartupLocation.Manual;

            // 使用 Dispatcher 延迟设置位置
            HoverTranslate.Dispatcher.InvokeAsync(async () =>
            {
                await HoverTranslate.getWord();
                // 获取鼠标屏幕坐标
                var mousePosition = System.Windows.Forms.Cursor.Position;

                // 获取屏幕工作区大小
                var screen = System.Windows.Forms.Screen.FromPoint(mousePosition);
                var workingArea = screen.WorkingArea;

                // 计算窗口的初始位置，使其居中于鼠标
                double windowWidth = HoverTranslate.Width;
                double windowHeight = HoverTranslate.Height;
                double left = mousePosition.X;
                double top = mousePosition.Y - windowHeight / 2;

                // 调整位置以确保窗口在屏幕内
                if (left < workingArea.Left)
                {
                    left = workingArea.Left;
                }
                else if (left + windowWidth > workingArea.Right)
                {
                    left = workingArea.Right - windowWidth;
                }

                if (top < workingArea.Top)
                {
                    top = workingArea.Top;
                }
                else if (top + windowHeight > workingArea.Bottom)
                {
                    top = workingArea.Bottom - windowHeight;
                }

                // 设置窗口的位置
                HoverTranslate.Left = left;
                HoverTranslate.Top = top;
                HoverTranslate.Visibility = Visibility.Visible;
            }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }
        public static void ShowOptionView()
        {
            var popupOptionView = new HandyControl.Controls.Window();
            popupOptionView.Content = new PopupConfigView();
            //popupOptionView.Topmost = true;
            popupOptionView.Show();
            //tbi.ShowCustomBalloon(new PopupConfigView(), PopupAnimation.Slide, null);
        }
        public static void Close()
        {
            DeskTopNotify.Hide();
        }

        //public static void KeyListens(HashSet<Keys> keys)
        //{
        //    string key = String.Join(',', keys.Select(k => KeyMap.keyMap.ContainsKey(k) ? KeyMap.keyMap[k] : ""));
        //    if (!string.IsNullOrEmpty(key))
        //    {
        //        var shortS = ServiceHelper.Services.GetService<IShortcutKeysService>();
        //        var findKey = shortS?.First(x => x.Key == key);
        //        if (findKey is not null && key.Equals(findKey.Keys, StringComparison.InvariantCulture))
        //        {
        //            if (findKey.ShortcutName == ShortcutName.OpenDeskTop)
        //            {
        //                Show();
        //            }
        //            else if (!string.IsNullOrEmpty(findKey.Method) && !string.IsNullOrEmpty(findKey.Interface))
        //            {
        //                dynamic? s = null;
        //                var type = flecion.getSingleType("MoqWord", findKey.Interface);
        //                var getService = typeof(IServiceProvider).GetMethod("GetService");
        //                if (getService is not null)
        //                {
        //                    s = getService.Invoke(ServiceHelper.Services, new object[] { type });
        //                }
        //                MethodInfo? m = type?.GetMethod(findKey.Method);
        //                if (s is not null && m is not null)
        //                {
        //                    m.Invoke(s, null);
        //                }
        //            }
        //        }
        //    }
        //}

        private static void DeskTopNotify_SourceInitialized(object? sender, EventArgs e)
        {
            // Get this window's handle
            IntPtr hWnd = new WindowInteropHelper(DeskTopNotify).Handle;

            // Get the extended window style
            int exStyle = (int)GetWindowLong(hWnd, GWL_EXSTYLE);

            // Set the WS_EX_TOOLWINDOW and WS_EX_TRANSPARENT styles
            exStyle |= WS_EX_TRANSPARENT;
            SetWindowLong(hWnd, GWL_EXSTYLE, (IntPtr)exStyle);
        }

        // Win32 API declarations
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x80;
        private const int WS_EX_TRANSPARENT = 0x20;

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
    }
}
