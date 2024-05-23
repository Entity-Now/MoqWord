using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace MoqWord.Helpers
{
    public static class JsonHelpaer
    {
        public static string ObjectToString(this object source)
        {
            return JsonSerializer.Serialize(source);
        }
        public static T StringToAny<T>(this string source)
        {
            return JsonSerializer.Deserialize<T>(source);
        }
    }
}
