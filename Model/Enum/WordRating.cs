using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Model.Enum
{
    public enum WordRating
    {
        [Description("再来一次")]
        Again = 1,
        [Description("困难的")]
        Hard = 2,
        [Description("很棒")]
        Good = 3,
        [Description("容易")]
        Easy = 4
    }
}
