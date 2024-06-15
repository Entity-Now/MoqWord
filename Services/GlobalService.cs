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
        Category? _currentCategory;
        /// <summary>
        /// 当前选择的词库
        /// </summary>
        public Category? currentCategory
        {
            get => _currentCategory;
            set{
                this.RaiseAndSetIfChanged(ref _currentCategory, value);
            }
        }
        Setting? _currentSetting;
        /// <summary>
        /// 配置
        /// </summary>
        public Setting? currentSetting
        {
            get => _currentSetting;
            set => this.RaiseAndSetIfChanged(ref _currentSetting, value);
        }



        public GlobalService(ICategoryService _categoryService, ISettingService _settingService, IPlayService _playService)
        {
            categoryService = _categoryService;
            settingService = _settingService;
            currentCategory = categoryService.IsSelectCategory();
            currentSetting = settingService.First(s => s.Id > 0);
            playService = _playService;
        }

        public Task Handle(CategoryNotify notification, CancellationToken cancellationToken)
        {
            currentCategory = categoryService.IsSelectCategory();
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
