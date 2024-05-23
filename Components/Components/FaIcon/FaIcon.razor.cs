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
        private string? ClassString => CssBuilder.Default("FaFont")
            .AddClass("Color HoverColor", !IsInherit)
            .AddClass("Hover", IsHover)
            .Build();
        private string? StyleString => CssBuilder.Default()
            .AddClass($"--color:{(IsInherit ? "inherit" : Color)};", !string.IsNullOrEmpty(Color))
            .AddClass($"--hover-color:{(IsInherit ? "inherit" : HoverColor)};", !string.IsNullOrEmpty(HoverColor))
            .AddClass($"--size:{Size};", !string.IsNullOrEmpty(Size))
            .Build();

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
    }
}
