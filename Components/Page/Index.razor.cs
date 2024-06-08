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
        protected IPlayService _playService { get; set; }
        [Inject]
        protected ICategoryService _categoryService { get; set; }

        public Word? CurrentWord => GetWordAtOffset(0);

        public Word? LastWord => GetWordAtOffset(1);

        public Word? PreviousWord => GetWordAtOffset(-1);

        private Word? GetWordAtOffset(int offset)
        {
            try
            {
                return _playService.ToDayWords[_playService.CurrentIndex + offset];
            }
            catch
            {
                return null;
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
