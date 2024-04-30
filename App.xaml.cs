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
            base.OnStartup(e);

            var mainWindow = ServiceHelper.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }

}
