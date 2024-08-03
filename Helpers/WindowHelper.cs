using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using Application = System.Windows.Application;

namespace MoqWord.Helpers
{
    public class WindowHelper
    { 
        private static bool _isMoving; 
        private static double _startMouseX; 
        private static double _startMouseY; 
        private static double _startWindLeft; 
        private static double _startWindTop;

        public static void Close(bool allWindow = true)
        {
            // 停止播放单词
            var playS = ServiceHelper.Services.GetService<IPlayService>();
            playS?.Stop(); // 加上空值检查

            // 停止应用程序
            if (allWindow)
            {
                var pro = Process.GetProcessesByName(Constants.ProgramName);
                foreach (var process in pro)
                {
                    try
                    {
                        process.Kill();
                        process.WaitForExit(); // 等待进程退出
                    }
                    catch (Exception ex)
                    {
                        // 记录或处理异常
                        Console.WriteLine($"Error terminating process: {ex.Message}");
                    }
                }
            }

            // 关闭活动窗口
            var window = GetActiveWindow();
            if (window != null)
            {
                window.Close();
            }
        }


        public static void Hide()
        {
            var window = GetActiveWindow(); if (window != null) { window.Visibility = Visibility.Hidden; window.Hide(); }
        }


        public static void Init()
        {
            DispatcherTimer dispatcherTimer = new(); dispatcherTimer.Tick += UpdateWindowPos; dispatcherTimer.Interval = TimeSpan.FromMilliseconds(17); dispatcherTimer.Start();
        }
        public static bool IsMaximized()
        {
            var window = GetActiveWindow(); if (window != null) { return window.WindowState == WindowState.Maximized; }
            return false;
        }

        public static void Maximize()
        {
            var window = GetActiveWindow(); if (window != null) { window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized; }
        }

        public static void Minimize()
        {
            var window = GetActiveWindow(); if (window != null) { window.WindowState = WindowState.Minimized; }
        }

        public static void Normal()
        {
            var window = GetActiveWindow(); if (window != null) { window.WindowState = WindowState.Normal; }
        }

        public static void Collpase()
        {
            var window = GetActiveWindow();
            if (window != null)
            {
                if (window.WindowState == WindowState.Normal)
                {
                    window.WindowState = WindowState.Maximized;
                }
                else if(window.WindowState == WindowState.Maximized)
                {
                    window.WindowState = WindowState.Normal;
                }else if (window.WindowState == WindowState.Minimized)
                {
                    window.WindowState = WindowState.Normal;
                }
            }
        }
        public static void StartMove()
        {
            _isMoving = true; _startMouseX = GetX(); _startMouseY = GetY(); var window = GetActiveWindow(); if (window == null) { return; }
            _startWindLeft = window.Left; _startWindTop = window.Top;
        }
        public static void StopMove()
        {
            _isMoving = false;
        }
        private static Window? GetActiveWindow()
        {
            return ServiceHelper.Services.GetService<MainWindow>();
        }

        private static int GetX()
        {
            return Control.MousePosition.X;
        }

        private static int GetY()
        {
            return Control.MousePosition.Y;
        }

        private static void UpdateWindowPos(object? sender, EventArgs e)
        {
            if (!_isMoving) { return; }
            double moveX = GetX() - _startMouseX; double moveY = GetY() - _startMouseY; Window? window = GetActiveWindow(); if (window == null) { return; }
            window.Left = _startWindLeft + moveX; window.Top = _startWindTop + moveY;
        }
    }
}