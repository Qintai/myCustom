using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using QinOpen.IApplicationBuilderExtend;
using QinOpen.Middleware;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace QinOpen
{
    /// <summary>
    ///   
    /// </summary>
    public class Startup
    {
        public IConfiguration _configuration { get; }

        public IWebHostEnvironment _env { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            //services.TryAddSingleton<IClientErrorFactory, ProblemDetailsClientErrorFactory>();
            services.TryAddSingleton<IClientErrorFactory, pp>();
            //services.TryAddSingleton<ObjectResult, Ae>();
            
            //services.AddSingleton<IClientErrorFactory, pp>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(new QinCommon.Common.Appsettings(_env.ContentRootPath));  //注入读取配置文件的类
            services.AddControllersWithViews();
            services.Swagger();
            services.Jwt();
            services.DbInitialization(_configuration);
            services.InjectionBusinessServer();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpContextAccessor accessor)
        {
            app.Use(next =>
            {
                return async context =>
                {
                    await next(context);
                    if (context.Response.StatusCode == 400)
                    {
                        //A a = new A();
                        //var dwy = a.ReadBodyAsync(context.Response);
                        //var result = dwy.Result;

                    //    {
                    //        string str = "";
                    //        var body = context.Response.Body;
                    //        using (StreamReader sr = new StreamReader(body, Encoding.UTF8, true, 1024, true))//这里注意Body部分不能随StreamReader一起释放
                    //        {
                    //             string ppo=sr.ReadToEnd();
                    //             // str = await sr.ReadToEndAsync();
                    //        }
                    //
                    //    }


                        {
                            //context.Response.Clear();
                            //context.Response.Body.Flush();
                            //context.Response.Body.Close();
                            //var result = Newtonsoft.Json.JsonConvert.SerializeObject(new MessageModel() { success = false, msg = "", code = context.Response.StatusCode.ToString()  });
                            //context.Response.ContentType = "application/json;charset=utf-8";
                            //await context.Response.WriteAsync(result);
                        }

                        //context.Response.OnStarting(() =>
                        //{
                        //    A a = new A();
                        //    var dwy = a.ReadBodyAsync(context.Response);
                        //    var result = dwy.Result;
                        //    return Task.CompletedTask;
                        //});
                        //context.Response.OnCompleted(() => 
                        //{
                        //    A a = new A();
                        //    var dwy = a.ReadBodyAsync(context.Response);
                        //    var result = dwy.Result;
                        //    return Task.CompletedTask;
                        //});
                    }
                };
            });


            //请求错误提示配置
            //app.UseErrorHandling();

            QinCommon.HttpContextUser.HttpContextHelper.Accessor = accessor;

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "myapi");
            });


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
