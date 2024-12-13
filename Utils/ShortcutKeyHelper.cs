using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Utils
{
    public static class ShortcutKeyHelper
    {
        public static IntPtr handle = IntPtr.Zero;
        public static void Register(IntPtr handle)
        {
            ShortcutKeyHelper.handle = handle;
            var services = ServiceHelper.getService().GetService<IShortcutKeysService>();
            services.All().ForEach(item =>
            {
                NativeMethod.RegisterHotKey(handle, 10086 + item.Id, (byte)item.Modifiers, item.Key);
            });
        }

        public static void UnRegister() 
        {
            var services = ServiceHelper.getService().GetService<IShortcutKeysService>();
            services.All().ForEach(item =>
            {
                NativeMethod.UnregisterHotKey(handle, 10086 + item.Id);
            });
        }

        public static void ReRegister()
        {
            UnRegister();
            Register(handle);
        }
    }
}
