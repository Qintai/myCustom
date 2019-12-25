using Microsoft.Extensions.DependencyInjection;
using System;
using QinCommon.Common;

namespace QinOpen
{
    /// <summary>
    /// Cors 启动服务
    /// </summary>
    public static class CorsSetup
    {
        public static void AddCorsSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));


            services.AddCors(o => o.AddPolicy("LimitRequests",
    p => p
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()));

            //services.AddCors(c =>
            //{
            //    c.AddPolicy("LimitRequests", policy =>
            //    {
            //        // 支持多个域名端口，注意端口号后不要带/斜杆：比如localhost:8000/，是错的
            //        // 注意，http://127.0.0.1:1818 和 http://localhost:1818 是不一样的，尽量写两个
            //        policy
            //        .WithOrigins(Appsettings.app(new string[] { "Startup", "Cors", "IPs" }).Split(','))
            //        .AllowAnyHeader()//Ensures that the policy allows any header.
            //        .AllowAnyMethod();
            //    });
            //});

        }
    }
}
