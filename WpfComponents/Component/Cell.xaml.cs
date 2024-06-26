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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoqWord.WpfComponents
{
    /// <summary>
    /// Cell.xaml 的交互逻辑
    /// </summary>
    public partial class Cell : UserControl
    {
        public Cell()
        {
            InitializeComponent();
        }

        public string Title { get; set; }
        public object operation { get; set; }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            this._title.Text = Title;
            this._opreation.Content = operation;
        }
    }
}
