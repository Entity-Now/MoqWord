using Microsoft.Extensions.DependencyInjection;
using MoqWord.Helpers;
using System.Configuration;
using System.Data;
using System.Windows;

namespace MoqWord
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // 注册键盘钩子
            KeyBoardHook.SetHook(KeyBoardHook._proc);
            base.OnStartup(e);

            var mainWindow = ServiceHelper.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
            // 卸载钩子
            KeyBoardHook.UnhookWindowsHookEx(KeyBoardHook._hookID);
        }
    }

}
