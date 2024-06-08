using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Model.Notify
{
    public class SettingNotify : INotification
    {
        public Setting setting { get; set; }
        public SettingNotify(Setting _setting) 
        {
            setting = _setting;
        }
    }
}
