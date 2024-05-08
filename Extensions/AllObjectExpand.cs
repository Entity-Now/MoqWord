using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Extensions
{
    public static class AllObjectExpand
    {
        public static object GetValue(this object _this, string name)
        {
            return _this?.GetType()?.GetProperty(name)?.GetValue(_this);
        }
        public static void SetValue(this object _this, string name, object value)
        {
            _this.GetType()?.GetProperty(name)?.SetValue(_this, value);
        }
    }
}
