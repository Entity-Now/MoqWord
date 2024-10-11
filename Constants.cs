using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoqWord
{
    public static class Constants
    {
        public const string Title = "莫欺单词";
        public const string ProgramName = "MoqWord";
        public const string Icon = "MoqWord.wwwroot.MOQ.ico";
        public const string ThemeAccent = "Light";
        public static string SelfPath = AppContext.BaseDirectory;
        public static string Connection = $"DataSource={Path.Combine(SelfPath, "MoqWord.db")}";
        public static string DistConf = $"{SelfPath}/Local/dist_conf.json";
        public static string BooksPath = $"{SelfPath}/Local/dicts/{{0}}";
    }
}
