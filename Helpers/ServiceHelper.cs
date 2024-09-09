using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DynamicData;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MoqWord.Components.Layout;
using MoqWord.Components.Page;
using MoqWord.ModelView;
using MoqWord.Repository.Interface;
using MoqWord.Services;
using MoqWord.Services.Interface;
using SqlSugar;
using Index = MoqWord.Components.Page.Index;

namespace MoqWord.Helpers
{
    internal class ServiceHelper
    {
        private static bool _isInitialized;
        private static IServiceCollection _serviceCollection;
        private static IServiceProvider _services;
        public static IServiceProvider Services { get { return getService(); } }
        public static IServiceCollection ServiceCollection { get { return getCollection(); } }


        private static void initializer()
        {
            _isInitialized = true;

            _serviceCollection = new ServiceCollection();
            _serviceCollection.AddMediatR(x =>
            {
                List<Assembly> assemblies = new List<Assembly>
                {
                    typeof(INotificationHandler<BookNotify>).Assembly,
                    typeof(INotificationHandler<SettingNotify>).Assembly
                };
                x.RegisterServicesFromAssemblies(assemblies.ToArray());
            });
            // 注入Mapster
            _serviceCollection.AddSingleton(TypeMappingConfig.getConfig());
            _serviceCollection.AddMapster();
            // 注册blazor wpf
            _serviceCollection.AddWpfBlazorWebView();
            _serviceCollection.AddBlazorWebViewDeveloperTools();
            // 注册 ant design
            _serviceCollection.AddAntDesign();
            _serviceCollection.AddSingleton<MainWindow>();
            // 注入scheduler 
            _serviceCollection.AddSingleton(SchedulerService.getScheduler());

            // 注入SQL Sugar仓储
            _serviceCollection.AddScoped(typeof(BaseRepository<>));
            // 注入SQL Sugar
            _serviceCollection.AddSingleton<ISqlSugarClient>(s =>
            {
                SqlSugarScope sqlSugar = new SqlSugarScope(new ConnectionConfig()
                {
                    DbType = SqlSugar.DbType.Sqlite,
                    ConnectionString = Constants.Connection,
                    IsAutoCloseConnection = true
                },
                db =>
                {
                    //每次上下文都会执行

                    //获取IOC对象不要求在一个上下文
                    //var log=s.GetService<Log>()

                    //获取IOC对象要求在一个上下文
                    //var appServive = s.GetService<IHttpContextAccessor>();
                    //var log= appServive?.HttpContext?.RequestServices.GetService<Log>();

                    db.Aop.OnLogExecuting = (sql, pars) =>
                    {

                    };
                });
                return sqlSugar;
            });
            // 注册页面

            // 注册服务
            _serviceCollection.AddTransient(typeof(BaseRepository<>));
            _serviceCollection.AddTransient<IBookRepository, BookRepository>();
            _serviceCollection.AddTransient<IPersonalRepository, PersonalRepository>();
            _serviceCollection.AddTransient<ISettingRepository, SettingRepository>();
            _serviceCollection.AddTransient<IWordRepository, WordRepository>();
            _serviceCollection.AddTransient<IWordLogRepository, WordLogRepository>();
            _serviceCollection.AddTransient<ITagRepository, TagRepository>();
            _serviceCollection.AddTransient<IPopupConfigRepository, PopupConfigRepository>();
            _serviceCollection.AddTransient<IShortcutKeysRepository, ShortcutKeysRepository>();
            _serviceCollection.AddTransient(typeof(BaseService<>));
            _serviceCollection.AddTransient<IBookService, BookService>();
            _serviceCollection.AddTransient<IPersonalService, PersonalService>();
            _serviceCollection.AddTransient<ISettingService, SettingService>();
            _serviceCollection.AddTransient<IWordService, WordService>();
            _serviceCollection.AddTransient<IWordLogService, WordLogService>();
            _serviceCollection.AddTransient<ITagService, TagService>();
            _serviceCollection.AddTransient<IPopupConfigService, PopupConfigService>();
            _serviceCollection.AddTransient<IShortcutKeysService, ShortcutKeysService>();
            _serviceCollection.AddTransient<DefaultPlaySound>();
            _serviceCollection.AddTransient<EdgePlaySound>();
            _serviceCollection.AddTransient<YoudaoPlaySound>();

            _serviceCollection.AddSingleton<IPlayService, PlayService>();
            _serviceCollection.AddSingleton(typeof(GlobalService));
            // 指定 GlobalService 实现的接口
            _serviceCollection.AddSingleton<INotificationHandler<BookNotify>>(provider => provider.GetService<GlobalService>());
            _serviceCollection.AddSingleton<INotificationHandler<SettingNotify>>(provider => provider.GetService<GlobalService>());
            // 注入scheduler
            _serviceCollection.AddSingleton(SchedulerService.getScheduler());
            // modelview
            _serviceCollection.AddSingleton(typeof(WordNotifyModelView));
            _serviceCollection.AddSingleton(typeof(PopupConfigModelView));
            _serviceCollection.AddSingleton(typeof(SettingModelView));

            _services = _serviceCollection.BuildServiceProvider();
        }

        public static IServiceProvider getService()
        {
            if (!_isInitialized)
            {
                initializer();
            }
            //return _containerBuilder.Resolve<T>();
            return _services;
        }
        public static IServiceCollection getCollection()
        {
            if (!_isInitialized)
            {
                initializer();
            }
            return _serviceCollection;
        }

        public static void Dispose()
        {
            _isInitialized = false;
        }
    }
}
