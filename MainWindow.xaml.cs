using Microsoft.Extensions.DependencyInjection;
using MoqWord.Helpers;
using MoqWord.Repository.Interface;
using MoqWord.Services;
using MoqWord.Utils;
using MoqWord.WpfComponents.Page;
using SqlSugar;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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
        public IShortcutKeysService shortcutKeysService { get; set; }
        public IPopupConfigService popupConfigService { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            InitHwnd();
            Resources.Add("services", ServiceHelper.getService());
            // 注册窗口事件类
            WindowHelper.Init();
        }
        public MainWindow(ISqlSugarClient _sqlSugar, ISettingRepository _settingRepository, IShortcutKeysService _shortcutKeysService, IPopupConfigService popupConfigService) : this()
        {
            sqlSugar = _sqlSugar;
            settingRepository = _settingRepository;
            shortcutKeysService = _shortcutKeysService;
            this.popupConfigService = popupConfigService;
            // 初始化数据库
            init();
            // 
            NotifyIconHelper.Icon();
            // 注册热键
            var handle = new WindowInteropHelper(this).Handle;
            ShortcutKeyHelper.Register(handle);
        }
        private void InitHwnd()
        {
            var helper = new WindowInteropHelper(this);
            helper.EnsureHandle();
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
                    SecondSoundName = "2",
                    SecondSoundSource = Sound.Youdao,
                    SpeechSpeed = 0,
                    SoundVolume = 100,
                    SuggestedCodec = "",
                    StartWithWindows = false,
                    SelectionInterval = 400
                });
            }
            // 初始化快捷键
            var firstShortKeys = shortcutKeysService.First();
            if (firstShortKeys is null or default(ShortcutKeys))
            {
                shortcutKeysService.InsertList(new List<ShortcutKeys>
                {
                    new ShortcutKeys
                    {
                        Name = "打开/关闭单词",
                        ShortcutName = "Ctrl,Alt,D",
                        Key = 68,
                        Modifiers = NativeMethod.KeyModifiers.Ctrl | NativeMethod.KeyModifiers.Alt
                    },
                    new ShortcutKeys
                    {
                        Name = "开始/停止播放",
                        ShortcutName = "Ctrl,Alt,P",
                        Key = 80,
                        Interface = "IPlayService",
                        Method = "Collapse",
                        Modifiers = NativeMethod.KeyModifiers.Ctrl | NativeMethod.KeyModifiers.Alt
                    },
                    new ShortcutKeys
                    {
                        Name = "上一个单词",
                        ShortcutName = "Ctrl,Alt,Left",
                        Key = 37,
                        Interface = "IPlayService",
                        Method = "Previous",
                        Modifiers = NativeMethod.KeyModifiers.Ctrl | NativeMethod.KeyModifiers.Alt
                    },
                    new ShortcutKeys
                    {
                        Name = "下一个单词",
                        ShortcutName = "Ctrl,Alt,Right",
                        Key = 39,
                        Interface = "IPlayService",
                        Method = "Next",
                        Modifiers = NativeMethod.KeyModifiers.Ctrl | NativeMethod.KeyModifiers.Alt
                    },
                    new ShortcutKeys
                    {
                        Name = "划词翻译",
                        ShortcutName = "Ctrl,Space",
                        Key = 32,
                        Modifiers = NativeMethod.KeyModifiers.Ctrl | NativeMethod.KeyModifiers.Alt
                    }
                });
            }
            // 初始化窗口样式
            var firstPopupConfig = popupConfigService.First();
            if (firstPopupConfig is null or default(PopupConfig))
            {
                popupConfigService.InsertOrUpdate(new PopupConfig()
                {
                    Background = "#B6EEEEEE",
                    Color = "#FFCC3737",
                    TranslationFontSize = 14.5,
                    WordNameFontSize = 32.5,
                    Opacity = 1,
                    IsLock = false,
                    IsPenetrate = false,
                    CreateDT = DateTime.Now,
                    UpdateDT = DateTime.Now,
                });
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var handle = new WindowInteropHelper(this).Handle;
            var source = HwndSource.FromHwnd(handle);
            source?.AddHook(WndProc);

        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handle)
        {
            if (msg != 0x0312)
            {
                return IntPtr.Zero;
            }
            var play = ServiceHelper.getService().GetService<IPlayService>();
            switch (wParam)
            {
                case 10087:
                    NotifyIconHelper.Show();
                    break;
                case 10088:
                    play?.Collapse();
                    break;
                case 10089:
                    play?.Previous();
                    break;
                case 10090:
                    play?.Next();
                    break;
                case 10091:
                    NotifyIconHelper.ShowHoverTranslate();
                    break;
            }

            return IntPtr.Zero;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }
    }
}