using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MoqWord.Repository;
using SqlSugar;

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
            // 注入Mapster
            _serviceCollection.AddMapster();
            _serviceCollection.AddSingleton(TypeMappingConfig.getConfig());
            // 注册blazor wpf
            _serviceCollection.AddWpfBlazorWebView();
            _serviceCollection.AddBlazorWebViewDeveloperTools();
            // 注册 ant design
            _serviceCollection.AddAntDesign();
            _serviceCollection.AddSingleton<MainWindow>();

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
            _serviceCollection.AddTransient(typeof(BaseRepository<>));
            _serviceCollection.AddTransient<ICategoryRepository, CategoryRepository>();
            _serviceCollection.AddTransient<IPersonalRepository, PersonalRepository>();
            _serviceCollection.AddTransient<ISettingRepository, SettingRepository>();
            _serviceCollection.AddTransient<IWordRepository, WordRepository>();
            _serviceCollection.AddTransient<IWordLogRepository, WordLogRepository>();

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
