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
    public class GlobalService : ReactiveObject, INotificationHandler<CategoryNotify>, INotificationHandler<SettingNotify>
    {
        ICategoryService categoryService { get; set; }
        ISettingService settingService { get; set; }
        public IPlayService playService { get; set; }

        /// <summary>
        /// 当前选择的词库
        /// </summary>
        Category? _currentCategory;
        public Category? currentCategory
        {
            get => _currentCategory;
            set{
                this.RaiseAndSetIfChanged(ref _currentCategory, value);
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

        public GlobalService(ICategoryService _categoryService, ISettingService _settingService, IPlayService _playService)
        {
            categoryService = _categoryService;
            settingService = _settingService;
            currentCategory = categoryService.IsSelectCategory();
            currentSetting = settingService.First(s => s.Id > 0);
            if (currentCategory is not null)
            {
                currentDay = categoryService.GetCurrentDay(currentCategory.Id);
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
            categoryService.SetGroupState(wordGroup, state);
        }

        public void SetRepeatCount(RepeatType repeatType)
        {
            currentSetting.RepeatCount = repeatType;
            settingService.SetRepeatCount(repeatType);
        }

        public Task Handle(CategoryNotify notification, CancellationToken cancellationToken)
        {
            currentCategory = categoryService.IsSelectCategory();
            currentDay = categoryService.GetCurrentDay(currentCategory.Id);
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
