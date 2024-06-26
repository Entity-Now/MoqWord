using HandyControl.Controls;
using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessageBox = HandyControl.Controls.MessageBox;
using ColorPickerH = HandyControl.Controls.ColorPicker;
using ReactiveUI;
using System.Runtime.CompilerServices;

namespace MoqWord.WpfComponents
{
    /// <summary>
    /// ColoPicker.xaml 的交互逻辑
    /// </summary>
    public partial class ColoPicker : UserControl
    {
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", 
                typeof(string), 
                typeof(ColoPicker),
                new FrameworkPropertyMetadata("#bebebe", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
            );

        public string Color
        {
            get => (string)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public ColoPicker()
        {
            InitializeComponent();
        }

        void SelectColorHandle(object sender, FunctionEventArgs<Color> e)
        {
            if (e.Info is Color c)
            {
                Color = c.ToString();
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var popup = new PopupWindow();
            var colorPicker = new ColorPickerH();
            colorPicker.SelectedColorChanged += SelectColorHandle;

            popup.Show(colorPicker);
            popup.Closed += (sender, e) =>
            {
                //colorPicker.SelectedColorChanged -= SelectColorHandle;
            };
        }
    }
}
