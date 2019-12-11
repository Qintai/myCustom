using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Newtonsoft.Json.Linq;
using QinCommon.Common.DB;
using QinServices;
using QinServices.Interface;

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


            #region SqlSuger

            SqlSugar.SqlSugarClient sqlsuger = new SqlSugar.SqlSugarClient(new SqlSugar.ConnectionConfig()
            {
                // "Server=localhost;Port=3306;Database=ut;Uid=root;Pwd=root;CharSet=utf8;CharSet=utf8;"
                ConnectionString = BaseDBConfig.ConnectionString,//必填, 数据库连接字符串
                DbType = (SqlSugar.DbType)BaseDBConfig.DbType,//必填, 数据库类型
                IsAutoCloseConnection = true,//默认false, 时候知道关闭数据库连接, 设置为true无需使用using或者Close操作
                InitKeyType = SqlSugar.InitKeyType.SystemTable//默认SystemTable, 字段信息读取, 如：该属性是不是主键，标识列等等信息
            });
            //  containerBuilder.Register(p => sqlsuger).AsSelf().InstancePerLifetimeScope(); //注册成作用域

            containerBuilder.Register<SqlSugar.SqlSugarClient>(p => sqlsuger).AsSelf(); //注册成作用域

            #endregion

            #region 业务的注入
            containerBuilder.Register(c => new CustomAutofacAop());//aop注册
            containerBuilder.RegisterType<A>().As<IA>().EnableInterfaceInterceptors();//AOP

            containerBuilder.RegisterType<zCustomUserRolesService>().As<IzCustomUserRolesService>();//瞬时
            containerBuilder.RegisterType<zCustomUserService>().As<IzCustomUserService>();//瞬时
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
