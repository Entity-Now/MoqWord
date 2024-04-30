using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Components.Layout
{
    public partial class MainLayout
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }
        public bool collapsed;

        public void toggle()
        {
            collapsed = !collapsed;
        }

        public void OnCollapse(bool isCollapsed)
        {
            Console.WriteLine($"Collapsed: {isCollapsed}");
        }

        public void NavigateGo(string path)
        {
            navigationManager.NavigateTo(path);
        }
    }
}
