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
        public EventWaitHandle? ProgramStarted;
        protected override void OnStartup(StartupEventArgs e)
        {
            bool createNew;
            ProgramStarted = new EventWaitHandle(false, EventResetMode.AutoReset, "MoqWord", out createNew);

            if (!createNew)
            {
                MessageBox.Show("请不要重复运行程序！");
                Environment.Exit(0);
            }

            var mainWindow = ServiceHelper.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }

}
