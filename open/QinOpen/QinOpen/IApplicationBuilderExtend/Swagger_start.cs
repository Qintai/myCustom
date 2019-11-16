﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace QinOpen.IApplicationBuilderExtend
{
    //https://www.cnblogs.com/taotaozhuanyong/p/11602820.html
    public static class Swagger_start
    {
        public static void Swagger(this IServiceCollection services)
        {
            // 预发行版本：Swashbuckle.AspNetCore ，拥有一切swagger的配置项
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                #region Core2.2的写法，已经注释

                // c.SwaggerDoc("QinOpen", new Info()
                // {
                //     Version = "1.0",
                //     Title = "QinOpen接口文档",
                //     Contact = new Contact() { Name="Api"},
                //     License = new License() {Name="QinApi",Url="www.google.com" }
                // });
                // c.OperationFilter<AddResponseHeadersFilter>();
                // c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                // c.OperationFilter<SecurityRequirementsOperationFilter>();

                //遍历出全部的版本，做文档信息展示
                //typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
                //{
                //    c.SwaggerDoc(version, new OpenApiInfo
                //    {
                //        Version = version,
                //        Title = $"{ApiName} 接口文档——Netcore 3.0",
                //        Description = $"{ApiName} HTTP API " + version,
                //        Contact = new OpenApiContact { Name = ApiName, Email = "Blog.Core@xxx.com", Url = new Uri("https://www.jianshu.com/u/94102b59cc2a") },
                //        License = new OpenApiLicense { Name = ApiName, Url = new Uri("https://www.jianshu.com/u/94102b59cc2a") }
                //    });
                //    c.OrderActionsBy(o => o.RelativePath);
                #endregion

            });
        }
    }
}