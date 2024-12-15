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
        // Windows API to get the DPI scale for the primary monitor
        [DllImport("user32.dll")]
        public static extern int GetDpiForWindow(IntPtr hwnd);

        public static void ShowHoverTranslate()
        {
            if (HoverTranslate.Visibility == Visibility.Visible)
            {
                HoverTranslate.Hide();
                return;
            }

            // 显示窗口
            HoverTranslate.SetCurrentString(GetWords.Get());
            HoverTranslate.WindowStartupLocation = WindowStartupLocation.Manual;

            // 显示窗口
            HoverTranslate.Show();

            // 使用 Dispatcher 延迟设置位置
            HoverTranslate.Dispatcher.InvokeAsync(async () =>
            {
                // 等待一帧，以确保控件的尺寸已更新
                // await Task.Delay(10);  // 确保控件的布局更新完成

                // 获取鼠标的屏幕坐标
                var mousePosition = System.Windows.Forms.Cursor.Position;

                // 获取当前窗口的 DPI 缩放比例
                var hwnd = new System.Windows.Interop.WindowInteropHelper(HoverTranslate).Handle;
                int dpi = GetDpiForWindow(hwnd);

                // 计算 DPI 缩放比例
                double scale = dpi / 96.0;  // 默认 96 DPI 是 100% 缩放

                // 使用 DPI 缩放来调整鼠标坐标
                double mouseX = mousePosition.X / scale;
                double mouseY = mousePosition.Y / scale;

                // 获取窗口的宽度和高度
                double windowWidth = HoverTranslate.Width;
                double windowHeight = HoverTranslate.Height;

                // 计算鼠标位置周围的剩余空间
                double spaceLeft = mouseX;
                double spaceRight = SystemParameters.WorkArea.Width - mouseX;
                double spaceTop = mouseY;
                double spaceBottom = SystemParameters.WorkArea.Height - mouseY;

                // 设置窗口的位置
                if (spaceRight < windowWidth)  // 如果右边空间不足
                {
                    HoverTranslate.Left = mouseX - windowWidth - 10;  // 显示在左边
                }
                else
                {
                    HoverTranslate.Left = mouseX + 10;  // 默认显示在右边
                }

                // 根据上下空间决定窗口的垂直位置
                if (spaceBottom < windowHeight)  // 如果下边空间不足
                {
                    HoverTranslate.Top = mouseY - windowHeight - 10;  // 显示在上边
                }
                else
                {
                    HoverTranslate.Top = mouseY + 10;  // 默认显示在下边
                }

                // 确保窗口不超出屏幕边界
                if (HoverTranslate.Left + HoverTranslate.Width > SystemParameters.WorkArea.Width)
                {
                    HoverTranslate.Left = SystemParameters.WorkArea.Width - HoverTranslate.Width;
                }

                if (HoverTranslate.Top + HoverTranslate.Height > SystemParameters.WorkArea.Height)
                {
                    HoverTranslate.Top = SystemParameters.WorkArea.Height - HoverTranslate.Height;
                }
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
