using System.Linq;
using System.Reflection;
using Chloe;
using Chloe.MySql;
using crud_base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace open.Api
{
    /// <summary>
    ///     配置数据库 的链接
    /// </summary>
    public static class BusinessInjection
    {
        public static void InjectionBusiness(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDbContext>(serviceProvider =>
            {
                return new MySqlContext(new DbConnectionFactory
                    (configuration.GetSection("con:consetting").Value));
            });
        }

        /// <summary>
        ///     批量注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void InjectionBusinessServer(this IServiceCollection services)
        {
            //便利整个程序集= crud-server
            var assemblies = GetAllAssembliesCoreWeb();
            var assemblye =
                assemblies.Where(k => k.FullName.Contains("crud-server")).FirstOrDefault(); //操作程序集 crud-server
            var list = assemblye.GetTypes().Where(t => t.DeclaringType == null //排除父类
                                                       && t.IsClass //是类还是委托 ，要排除委托
                                                       && !t.IsGenericType) //排除了泛型类 ，接口不是类，也会排除掉
                .ToList(); //将 Assembly 的反射Types，得到程序集的所有类型
            foreach (var item in list)
            {
                var inters = item.GetInterfaces().Where(k => !k.Name.Contains("IBaseMysql")).ToArray();
                if (inters == null || inters.Count() == 0)
                    services.AddTransient(item); //注入业务类
                else
                    foreach (var inter in inters)
                        services.AddTransient(inter, item); //左边是接口 右边是实现类
            }
            services.AddTransient(typeof(AjaxResult));
            // services.AddTransient<IzUserSer, zUserSer>(); //注入业务层--接口 实现类
            // services.AddTransient(typeof(zmyhorkSer));//注入业务类
            //  ExceptionHandler .net老版本的全局filter
        }

        /// <summary>
        ///     获取Asp.Net Core项目所有程序集
        /// </summary>
        /// <returns></returns>
        public static Assembly[] GetAllAssembliesCoreWeb()
        {
            var executingAssembly = Assembly.GetExecutingAssembly(); //当前程序集
            var assemblies = executingAssembly
                .GetReferencedAssemblies() //获取所有引用的程序集的System.Reflection.AssemblyName对象 在这个集会上。程序集的名称
                .Select(Assembly.Load) //在给定System.Reflection.AssemblyName的情况下加载程序集。
                .Where(m => m.FullName.Contains("crud-server"))
                .ToList();
            //2.获取启动入口程序集（Ray.EssayNotes.AutoFac.CoreApi）
            var
                assembly = Assembly
                    .GetEntryAssembly(); //获取默认应用程序域中可执行的进程。在其他应用中   域，这是系统执行的第一个可执行文件。应用程序域。执行程序集(系统。字符串)。
            assemblies.Add(assembly); //把当前运行的程序集也添加进来
            return assemblies.ToArray(); // assemblies=crud-server+crud-web
        }
    }
}