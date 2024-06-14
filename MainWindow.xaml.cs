using MoqWord.Helpers;
using MoqWord.Repository.Interface;
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
        public ISettingRepository settingRepository { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Resources.Add("services", ServiceHelper.getService());
            // 注册窗口事件类
            WindowHelper.Init();
            // 
            NotifyIconHelper.Icon();
        }
        public MainWindow(ISqlSugarClient _sqlSugar, ISettingRepository _settingRepository) : this()
        {
            sqlSugar = _sqlSugar;
            settingRepository = _settingRepository;
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
            // 初始化setting配置
            var firstSetting = settingRepository.GetSingle(x => x.Id >= 0);
            if (firstSetting is null or default(Setting))
            {
                settingRepository.Insert(new Setting
                {
                    DarkNess = false,
                    UsingSound = false,
                    EverDayCount = 20,
                    RepeatCount = RepeatType.Three,
                    TimeInterval = 365,
                    DesiredRetension = 0.9,
                    Difficulty = 0.5,
                    SoundName = "",
                    SoundSource = Sound.Default,
                    SpeechSpeed = 0,
                    SoundVolume = 100,
                    SuggestedCodec = "",
                    StartWithWindows = false
                });
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}