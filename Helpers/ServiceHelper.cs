using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Builder;
using Autofac.Extensions.DependencyInjection;
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
            // 注册blazor wpf
            _serviceCollection.AddWpfBlazorWebView();
            _serviceCollection.AddBlazorWebViewDeveloperTools();
            // 注册 ant design
            _serviceCollection.AddAntDesign();
            _serviceCollection.AddAutofac(Configuration);
            _serviceCollection.AddSingleton<MainWindow>();

            _services = _serviceCollection.BuildServiceProvider();
        }

        private static void Configuration(ContainerBuilder builder)
        {
            // 注入SQL Sugar仓储
            builder.RegisterGeneric(typeof(IBaseRepository<>));
            // 注入SQL Sugar
            builder.Register<ISqlSugarClient>(c =>
            {
                SqlSugarScope sqlSugar = new SqlSugarScope(new ConnectionConfig()
                {
                    DbType = SqlSugar.DbType.Sqlite,
                    ConnectionString = Constants.Connection,
                    IsAutoCloseConnection = true,
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
            // 注入规则
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                // 按照实现的接口进行注册
                .AsImplementedInterfaces()
            // 扫描符合条件的类
                .Where(x => x.Name.EndsWith("Service") || x.Name.EndsWith("Repository") || x.Name.EndsWith("Controller"))
                // 属性注入
                .PropertiesAutowired()
                // 将自身注册为服务
                .AsSelf()
                // 每次解析服务都创建一个新的实例
                .InstancePerDependency();
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
