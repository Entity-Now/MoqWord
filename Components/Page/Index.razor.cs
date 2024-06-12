using AntDesign;
using MediatR;
using Microsoft.AspNetCore.Components;
using MoqWord.Model.Notify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqWord.Components.Page
{
    public partial class Index : INotificationHandler<WordNotify>
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }
        [Inject]
        protected IMessageService _message { get; set; }
        [Inject]
        protected ICategoryService _categoryService { get; set; }
        [Inject]
        public IPlayService playService
        {
            get => ViewModel;
            set
            {
                if (value is PlayService _val)
                {
                    ViewModel = _val;
                }
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            var SelectIsTrue = _categoryService.IsSelectCategory();
            if (SelectIsTrue is null)
            {
                _message.Warning("请选择单词~");
                navigationManager.NavigateTo("/WordList");
            }
        }

        public Task Handle(WordNotify notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
