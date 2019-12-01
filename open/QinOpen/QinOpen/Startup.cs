using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using QinCommon.Common;
using QinOpen.Filter;
using QinOpen.Middleware;

namespace QinOpen
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        public IWebHostEnvironment _env { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        /// <summary>
        /// AutoFac
        /// </summary>
        /// <param name="containerBuilder"></param>
        public void ConfigureServices(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<CustomAutofacModule>();
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(a =>
              {
                  a.Filters.Add<GlobalAction>();
                  a.EnableEndpointRouting = false;
              }); // GlobalAction  测试 ActionFileer，
            services.AddControllers(o =>
            {
                o.Filters.Add(typeof(GlobalExceptionsFilter)); // 全局异常过滤
            })
                //全局配置Json序列化处理
                .AddNewtonsoftJson(options =>
                {
                    //忽略循环引用
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    //不使用驼峰样式的key
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    //设置时间格式
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
                });
            services.AddSingleton<IClientErrorFactory, MyIClientErrorFactory>(); // 默认的实现是 ProblemDetailsClientErrorFactory
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(new Appsettings(_env.ContentRootPath));  //注入读取配置文件的类
            services.AddControllersWithViews();
            services.Swagger();
            services.Jwt();
            //services.AddCustomAuthorization();
            services.DbInitialization(_configuration);
            services.InjectionBusinessServer();

            #region 关于ApiBehaviorOptions的说明
            /*
            MvcCoreServiceCollectionExtensions：
                       services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<ApiBehaviorOptions>, ApiBehaviorOptionsSetup>());
                       初始化 ApiBehaviorOptions接口，直接使用 ApiBehaviorOptionsSetup实现类，
                       而 ApiBehaviorOptionsSetup在实例化的时候，就直接写死 new BadRequestObjectResult
                       当任何人使用 ApiBehaviorOptionsSetup 时，最后使用的就是 BadRequestBojectResult
                       比如 ：ModelStateInvalidFilterFactory，使用时需要 IOptions<ApiBehaviorOptions>，传给ModelStateInvalidFilter去实例化，
                   ModelStateInvalidFilter 有个短路方法，
                   public void OnActionExecuting(ActionExecutingContext context)
                   {     
                       _apiBehaviorOptions 就是上一级传输的值
                       if (context.Result == null && !context.ModelState.IsValid)
                       {
                           _logger.ModelStateInvalidFilterExecuting();
                           context.Result = _apiBehaviorOptions.InvalidModelStateResponseFactory(context); //最后执行了 BadRequestObjectResult
                       }
                   }
            */
            #endregion

            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true); //禁用自带 的验证

            services.AddSingleton<DynamicRoute>(); //自定义路由用的上的

            services.AddRouting(options => 
            {
                
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpContextAccessor accessor)
        {
            app.UseErrorHandling();//请求错误提示配置
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

            #region 后缀带有html的拦截处理
            //app.Map("/.html", builder =>
            //{        // 带“/” 的处理，
            //    builder.Run(async context =>
            //    {
            //        string contentRootPath = _hostingEnvironment.ContentRootPath + "\\Html";
            //        string filename = context.Request.Path.Value.Substring(context.Request.Path.Value.LastIndexOf("/") + 1).Replace(".html", "");
            //        string filepath = $"{contentRootPath}\\{filename}\\{filename}.html";
            //        await context.Response.WriteAsync(File.ReadAllText(filepath), Encoding.UTF8);
            //    });
            //});

            app.MapWhen(context =>
            {
                if (context.Request.Path.Value.Contains(".html"))
                    return true;
                else
                    return false;
            }, builder =>
            {
                builder.Run(async context =>
                {
                    string contentRootPath = _env.ContentRootPath + "\\Html";
                    string filename = context.Request.Path.Value.Substring(context.Request.Path.Value.LastIndexOf("/") + 1).Replace(".html", "");
                    string filepath = $"{contentRootPath}\\{filename}\\{filename}.html";
                    if (System.IO.File.Exists(filepath))
                        await context.Response.WriteAsync(System.IO.File.ReadAllText(filepath), System.Text.Encoding.UTF8);
                    else
                        await context.Response.WriteAsync("文件不存在！", System.Text.Encoding.UTF8);
                });
            });

            // 二：路由式解决，输入地址有限制，不能适应更加复杂的情况
            //app.UseRouter(router =>
            //{
            //    Action<IApplicationBuilder> must = route =>
            //    {
            //        route.Run(async context =>
            //        {
            //            string contentRootPath = _hostingEnvironment.ContentRootPath + "\\Html";
            //            string routename = context.GetRouteValue("some").ToString();
            //            string filepath = $"{contentRootPath}\\{routename}\\{routename}.html";
            //            await context.Response.WriteAsync(File.ReadAllText(filepath), Encoding.UTF8);
            //            //using (StreamWriter streamWriter = new StreamWriter(context.Response.Body, Encoding.UTF8))
            //            //    streamWriter.Write("你的token已经过期了");
            //        });
            //    };
            //    router.MapMiddlewareRoute("{some}.html", must);
            //    router.MapMiddlewareRoute("{a?}/{some}.html", must);
            //});

            #endregion

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();//先开启认证
            app.UseAuthorization(); //后进行授权

            app.UseEndpoints(endpoints =>
            {
                // 先要有这个 ：services.AddSingleton<DynamicRoute>();
                //再写的下面的 自定义路由，
                endpoints.MapDynamicControllerRoute<DynamicRoute>("{some}.ip");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
