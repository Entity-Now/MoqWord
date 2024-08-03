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
using ReactiveUI;

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

        public bool dayViewCollapsed { get; set; }
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

        private void toggle()
        {
            collapsed = !collapsed;
        }

        private void OnCollapse(bool isCollapsed)
        {
            Console.WriteLine($"Collapsed: {isCollapsed}");
        }
        private void SelectGroupNumber(int number)
        {
            ViewModel.SelectGroupNumber(number);
            dayViewCollapsed = !dayViewCollapsed;
        }
        private void SetGroupState(WordGroup wordGroup, bool state)
        {
            wordGroup.IsGrasp = state;
            ViewModel.SetGroupState(wordGroup, state);
            dayViewCollapsed = !dayViewCollapsed;
        }
        private void SetRepeatCount(RepeatType repeatType)
        {
            globalService.SetRepeatCount(repeatType);
        }
        private void NavigateGo(string path)
        {
            navigationManager.NavigateTo(path);
        }
        private void LocationChangeHandle(object sender, LocationChangedEventArgs e)
        {
            currentKey = e.Location.Split(@"/")[^1];
            InvokeAsync(StateHasChanged);
        }
        private void Dispose()
        {
            navigationManager.LocationChanged -= LocationChangeHandle;
        }
    }
}
