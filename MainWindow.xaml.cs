using MoqWord.Helpers;
using SqlSugar;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoqWord
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ISqlSugarClient sqlSugar { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Resources.Add("services", ServiceHelper.getService());
            // 注册窗口事件类
            WindowHelper.Init();
        }
        public MainWindow(ISqlSugarClient _sqlSugar) : this()
        {
            sqlSugar = _sqlSugar;
            // 初始化数据库
            init();
        }
        private void init()
        {
            if (sqlSugar.DbMaintenance.CreateDatabase())
            {
                sqlSugar.CodeFirst.InitTables(Assembly.GetExecutingAssembly().GetTypes().Where(x => x.GetCustomAttributes<SugarTable>().Any()).ToArray());
            }
            else
            {
                MessageBox.Show("创建失败");
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}