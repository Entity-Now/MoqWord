using MoqWord.Attributes;
using MoqWord.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Model.Data
{
    public class SettingItem
    {
        public string PropertyName { get; set; }
        public object Value { get; set; }
        public CellTypeAttribute Cell { get; set; }
        public Action<object> Handle { get; set; }
    }
}
