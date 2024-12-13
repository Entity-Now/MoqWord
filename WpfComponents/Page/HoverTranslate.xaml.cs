using EnTranslate.utility;
using Microsoft.Extensions.DependencyInjection;
using MoqWord.ModelView;
using MoqWord.Services.Interface;
using MoqWord.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MoqWord.WpfComponents.Page
{
    /// <summary>
    /// HoverTranslate.xaml 的交互逻辑
    /// </summary>
    public partial class HoverTranslate : Window
    {
        public string CurrentString { get; set; } = string.Empty;
        ISettingService settingService;
        public HoverTranslate()
        {
            InitializeComponent();
            DataContext = ServiceHelper.Services.GetService<WordNotifyModelView>();
            settingService = ServiceHelper.Services.GetService<ISettingService>();
        }

        public async Task getWord()
        {
            try
            {
                var Trans = new List<string>() { CurrentString };
                var result = await TranslateHelper.getTranslateAsync(settingService.First().TranslateType, Trans);
                this.translateText.Text = string.Join('\n', result);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private async void Selection_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;

            GetWords.Set(this.translateText.Text);
        }

        private async void Play_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (settingService is null)
                {
                    MessageBox.Show("配置获取失败!");
                    return;
                }
                (IPlaySound? sound, IPlaySound? _) = settingService.getCurrentSound();
                if (sound is null)
                {
                    MessageBox.Show("音源获取失败！！");
                    return;
                }
                await sound.PlayAsync(this.translateText.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
