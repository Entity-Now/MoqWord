using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace MoqWord.Helpers
{
    public static class NotifyIconHelper
    {
        public static TaskbarIcon tbi { get; set; }
        public static void Icon()
        {
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
                        Command = ReactiveCommand.Create(() =>
                        {
                            var window = ServiceHelper.Services.GetService<MainWindow>();
                            window.WindowState = WindowState.Normal;
                            window.Topmost = true;
                            window.Topmost = false;
                        })
                    }
                }
            };
            tbi.ContextMenu = contextMenu;
            
        }

        public static void Show()
        {
            if (tbi.CustomBalloon is default(Popup) or null)
            {
                tbi.ShowCustomBalloon(new WordPopup(), System.Windows.Controls.Primitives.PopupAnimation.Slide, null);
            }
            else
            {
                Close();
            }
            
        }
        public static void Close()
        {
            tbi.CloseBalloon();
        }
    }
}
