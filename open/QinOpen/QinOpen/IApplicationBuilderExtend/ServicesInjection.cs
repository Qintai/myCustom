using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reflection;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace QinOpen.IApplicationBuilderExtend
{
    /// <summary>
    /// 读取Json的配置，自定义Json的配置，json文件内容，通过IOptions，映射成实体
    /// https://www.cnblogs.com/dotnet261010/p/10172961.html 
    /// https://www.cnblogs.com/CreateMyself/p/6859076.html
    /// </summary>
    public static class ServicesInjection
    {
        public static void InjectionBusinessServer(this IServiceCollection services)
        {
            #region 在当前QinOpen程序集上，动态注入实例 InjectionBusinessServer.json，Example=不带接口，InterExample带接口的

            string text = File.ReadAllText("IApplicationBuilderExtend/InjectionBusinessServer.json");
            var jobject = JObject.Parse(text);
            if (jobject == null || jobject.Count == 0)
                return;
            foreach (var item in jobject["Example"])
            {
                string name = ((JProperty)item).Name;
                Type tt = Type.GetType(name, true, true);
                services.AddTransient(tt);
            }
            foreach (var item in jobject["InterExample"])
            {
                string abstractname = ((JProperty)item).Name;
                string Realizationname = ((JProperty)item).Value.ToString();
                Type tt = Type.GetType(abstractname, true, true);
                Type aa = Type.GetType(Realizationname, true, true);
                services.AddTransient(tt, aa);
            }
            //services.AddTransient<ClientErrorData, MessageModel>();

            
            #endregion
        }

    }
}
