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

namespace MoqWord.Helpers
{
    public static class NotifyIconHelper
    {
        public static Window DeskTopNotify { get; set; }
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

        static NotifyIconHelper()
        {
            DeskTopNotify = new DeskTop();
            //DeskTopNotify.Content = new WordPopup();
            //DeskTopNotify.ShowInTaskbar = false; // 不显示在任务栏中
            //DeskTopNotify.AllowsTransparency = true; // 允许透明
            //DeskTopNotify.WindowStyle = WindowStyle.None; // 无边框样式
            //DeskTopNotify.Topmost = true;
            //DeskTopNotify.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255)); // 设置背景为透明
            //DeskTopNotify.MouseLeftButtonDown += (sender, e) =>
            //{
            //    DeskTopNotify.DragMove();
            //};
            //DeskTopNotify.SourceInitialized += DeskTopNotify_SourceInitialized;

            #region 键盘钩子
            // 设置钩子
            KeyBoardHook.KeysHandle += (keys) =>
            {
                string key = String.Join(',', keys.Select(k => k.ToString()));
                if (!string.IsNullOrEmpty(key))
                {
                    var shortS = ServiceHelper.Services.GetService<IShortcutKeysService>();
                    var findKey = shortS.First(x => x.Keys == key && x.Name == "");
                    if (findKey is not null && key.Equals(findKey.Keys) && findKey.ShortcutName == ShortcutName.OpenDeskTop)
                    {
                        Show();
                    }
                }
            };
            #endregion
        }

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
