using MediatR;
using MoqWord.Model.Notify;
using MoqWord.Services.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ReactiveUI.Blazor;
using ReactiveUI;

namespace MoqWord.Services
{
    public class GlobalService : ReactiveObject, INotificationHandler<BookNotify>, INotificationHandler<SettingNotify>
    {
        IBookService BookService { get; set; }
        ISettingService settingService { get; set; }
        public IPlayService playService { get; set; }

        /// <summary>
        /// 当前选择的词库
        /// </summary>
        Book? _currentBook;
        public Book? currentBook
        {
            get => _currentBook;
            set{
                this.RaiseAndSetIfChanged(ref _currentBook, value);
            }
        }
        /// <summary>
        /// 配置
        /// </summary>
        Setting? _currentSetting;
        public Setting? currentSetting
        {
            get => _currentSetting;
            set => this.RaiseAndSetIfChanged(ref _currentSetting, value);
        }
        /// <summary>
        /// 天数
        /// </summary>
        int _currentDay = 1;
        public int currentDay
        {
            get => _currentDay;
            set => this.RaiseAndSetIfChanged(ref _currentDay, value);
        }

        public GlobalService(IBookService _BookService, ISettingService _settingService, IPlayService _playService)
        {
            BookService = _BookService;
            settingService = _settingService;
            currentBook = BookService.IsSelectBook();
            currentSetting = settingService.First(s => s.Id > 0);
            if (currentBook is not null)
            {
                currentDay = BookService.GetCurrentDay(currentBook.Id);
            }
            playService = _playService;

            this.WhenAnyValue(v => v.currentDay)
                .Subscribe((day) =>
                {
                    playService.Init(day);
                });
        }

        public void SelectGroupNumber(int groupNumber)
        {
            currentDay = groupNumber;
        }

        public void SetGroupState(WordGroup wordGroup, bool state)
        {
            BookService.SetGroupState(wordGroup, state);
        }

        public void SetRepeatCount(RepeatType repeatType)
        {
            currentSetting.RepeatCount = repeatType;
            settingService.SetRepeatCount(repeatType);
        }

        public Task Handle(BookNotify notification, CancellationToken cancellationToken)
        {
            currentBook = BookService.IsSelectBook();
            currentDay = BookService.GetCurrentDay(currentBook.Id);
            playService.Init();
            return Task.CompletedTask;
        }

        public Task Handle(SettingNotify notification, CancellationToken cancellationToken)
        {
            currentSetting = settingService.First(s => s.Id > 0);
            return Task.CompletedTask;
        }
    }
}
