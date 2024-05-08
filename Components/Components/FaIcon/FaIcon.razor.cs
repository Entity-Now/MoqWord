using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Components.Components.FaIcon
{
    public partial class FaIcon : AntDomComponentBase
    {
        [Parameter]
        public string Type { get; set; }
        [Parameter]
        public string Color { get; set; }
        [Parameter]
        public string HoverColor { get; set; }
        [Parameter]
        public string Size { get; set; }
        [Parameter]
        public bool IsHover { get; set; } = true;
        [Parameter]
        public bool IsInherit { get; set; } = false;

        // 定义点击事件参数
        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        // 定义点击事件处理方法
        private async Task OnClickHandler(MouseEventArgs e)
        {
            // 调用点击事件参数
            await OnClick.InvokeAsync(e);
        }

        private string getVar()
        {
            string val = "";
            if (IsInherit == true || !string.IsNullOrEmpty(Color))
            {
                val += $"--color:{(IsInherit ? "inherit" : Color)};";
            }
            if (IsInherit == true || !string.IsNullOrEmpty(HoverColor))
            {
                val += $"--hover-color:{(IsInherit ? "inherit" : HoverColor)};";
            }
            if (!string.IsNullOrEmpty(Size))
            {
                val += $"--size:{Size};";
            }
            return val;
        }
    }
}
