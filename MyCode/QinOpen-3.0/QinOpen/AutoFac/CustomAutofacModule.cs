using Autofac;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Newtonsoft.Json.Linq;
using QinEntity;
using IQinRepository;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace QinOpen
{
    /// <summary>
    /// AutoFac 批量注入
    /// </summary>
    public class CustomAutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {

            ApplicationPartManager manager = new ApplicationPartManager();
            Assembly assembly = this.GetType().GetTypeInfo().Assembly;
            manager.ApplicationParts.Add(new AssemblyPart(assembly));
            manager.FeatureProviders.Add(new ControllerFeatureProvider());
            var feature = new ControllerFeature();
            manager.PopulateFeature(feature);

            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<ApplicationPartManager>().AsSelf().SingleInstance();//单例
            builder.RegisterTypes(feature.Controllers.Select(ti => ti.AsType()).ToArray()).PropertiesAutowired();

            
            #region 业务的注入
            containerBuilder.Register(c => new CustomAutofacAop());//aop注册
            containerBuilder.RegisterType<A>().As<IA>().EnableInterfaceInterceptors();//AOP
            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;

            //1-先注册sqlsuger，在 方法    services.DbInitialization(_configuration); 中已经注册
            //2- 再注册仓储Repository 
            #region 泛型注入，没能实现，泛型的仓储接口 → 泛型的仓储实现
            // Type ty = assemblysRepository.GetTypes().FirstOrDefault();
            //   containerBuilder.RegisterGeneric(ty).As(typeof(IBaseRepository<>))
            //   .InstancePerDependency();//注册仓储泛型


            //containerBuilder.RegisterType<SqlSugerHelper<zCustomUserRoles>>().As<IBaseRepository<zCustomUserRoles>>();
            //containerBuilder.RegisterType<SqlSugerHelper<zCustomUser>>().As<IBaseRepository<zCustomUser>>();
            // 因为 完全解耦了，SqlSugerHelper这个是仓储管理员的具体实现-是泛型类
            // 在这里是不能访问 SqlSugerHelper 他的
            // 为了实现这两句代码，只能再创建“具体”的仓储管理员，才能实现，
            // 

            #endregion

            var repositoryDllFile = Path.Combine(basePath, "QinRepository.dll");
            var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            containerBuilder.RegisterAssemblyTypes(assemblysRepository)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope(); ;

            //3-再注入服务
            var servicesDllFile = Path.Combine(basePath, "QinServices.dll");
            var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            containerBuilder.RegisterAssemblyTypes(assemblysServices)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            {
                //之前的写法
                //containerBuilder.RegisterType<zCustomUserRolesService>().As<IzCustomUserRolesService>();
                //containerBuilder.RegisterType<zCustomUserService>().As<IzCustomUserService>();
            }


            /*
                这样弄彻底解耦项目，直接加载dll文件，如果你新建一个table，你要建立这些类
                TableEntity（实体）→
                ItableRepository（接口）→tableRepository（实现类）→
                ItableService（接口）→tableService（实现类）
                总共3个接口，2个类

             */

            #endregion

            #region 注入当前程序集的
            string text = File.ReadAllText("BuilderExtend/InjectionBusinessServer.json");
            var jobject = JObject.Parse(text);
            if (jobject == null || jobject.Count == 0)
                return;
            foreach (var item in jobject["Example"])
            {
                string name = ((JProperty)item).Name;
                Type tt = Type.GetType(name, true, true);
                containerBuilder.Register(c => tt);
            }
            foreach (var item in jobject["InterExample"])
            {
                string abstractname = ((JProperty)item).Name;
                string Realizationname = ((JProperty)item).Value.ToString();
                Type tt = Type.GetType(abstractname, true, true);
                Type aa = Type.GetType(Realizationname, true, true);
                containerBuilder.RegisterType(tt).As(aa);
            }
            #endregion

            containerBuilder.Register(p => new MessageModel()).AsSelf().SingleInstance();

        }
    }
}
