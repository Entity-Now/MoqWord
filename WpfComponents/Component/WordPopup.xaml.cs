using Edge_tts_sharp;
using Microsoft.Extensions.DependencyInjection;
using MoqWord.ModelView;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoqWord.WpfComponents
{
    /// <summary>
    /// WordPopup.xaml 的交互逻辑
    /// </summary>
    public partial class WordPopup : UserControl, IViewFor<WordNotifyModelView>
    {
        public WordPopup()
        {
            InitializeComponent();
            DataContext = ServiceHelper.Services.GetService<WordNotifyModelView>();
            this.WhenAnyValue(x => x.ViewMode.popupConfigModelView.IsLock)
                .Subscribe((e) =>
                {
                    if (e)
                    {
                        if (_border.Background is not null)
                        {
                            _border.Background.Opacity = 0;
                        }
                        else
                        {
                            _border.Background = new SolidColorBrush(Colors.Transparent);
                        }
                        _tools.Opacity = 0;
                        _LockButton.Opacity = 1;
                    }
                    else
                    {
                        if (_border.Background is not null)
                        {
                            _border.Background.Opacity = 1;
                        }
                        else
                        {
                            _border.Background = new SolidColorBrush(Colors.Transparent);
                        }
                        _tools.Opacity = 1;
                        _LockButton.Opacity = 0;
                    }
                });
            this.WhenAnyValue(x => x.ViewMode.playService.IsLoopPlay)
                .Subscribe((e) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        if (e)
                        {
                            stopButton.IsEnabled = true;
                            stopButton.Visibility = Visibility.Visible;
                            loopButton.IsEnabled = false;
                            loopButton.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            stopButton.IsEnabled = false;
                            stopButton.Visibility = Visibility.Collapsed;
                            loopButton.IsEnabled = true;
                            loopButton.Visibility = Visibility.Visible;
                        }
                    });
                });
        }

        public WordNotifyModelView? ViewMode
        {
            get => DataContext as WordNotifyModelView;
            set => DataContext = value;
        }
        public WordNotifyModelView? ViewModel { get; set; }
        object? IViewFor.ViewModel 
        {
            get
            {
                return ViewModel;
            }
            set
            {
                ViewModel = (WordNotifyModelView)value;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("hello");
        }
    }
}
