using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoqWord.Helpers;
using MoqWord.Components.Components.FaIcon;

using AntDesign;
using System.Windows;

namespace MoqWord.Components.Layout
{
    public partial class MainLayout
    {
        string currentKey { get; set; }
        [Inject]
        protected NavigationManager navigationManager { get; set; }
        [Inject]
        protected IconService iconService { get; set; }
        [Inject]
        protected GlobalService globalService
        {
            get => ViewModel;
            set => ViewModel = value;
        }

        public bool collapsed { get; set; }

        protected override async Task OnInitializedAsync()
        {
            globalService.PropertyChanged += async (sender, e) =>
            {
                await InvokeAsync(StateHasChanged);
            };
            await base.OnInitializedAsync();
            currentKey = navigationManager.Uri.Split(@"/")[^1];
            navigationManager.LocationChanged += LocationChangeHandle;
        }

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
        void LocationChangeHandle(object sender, LocationChangedEventArgs e)
        {
            currentKey = e.Location.Split(@"/")[^1];
            InvokeAsync(StateHasChanged);
        }
        public void Dispose()
        {
            navigationManager.LocationChanged -= LocationChangeHandle;
        }
    }
}
