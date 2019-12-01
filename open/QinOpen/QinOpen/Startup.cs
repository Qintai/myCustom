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
              }); // GlobalAction  ���� ActionFileer��
            services.AddControllers(o =>
            {
                o.Filters.Add(typeof(GlobalExceptionsFilter)); // ȫ���쳣����
            })
                //ȫ������Json���л�����
                .AddNewtonsoftJson(options =>
                {
                    //����ѭ������
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    //��ʹ���շ���ʽ��key
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    //����ʱ���ʽ
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
                });
            services.AddSingleton<IClientErrorFactory, MyIClientErrorFactory>(); // Ĭ�ϵ�ʵ���� ProblemDetailsClientErrorFactory
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(new Appsettings(_env.ContentRootPath));  //ע���ȡ�����ļ�����
            services.AddControllersWithViews();
            services.Swagger();
            services.Jwt();
            //services.AddCustomAuthorization();
            services.DbInitialization(_configuration);
            services.InjectionBusinessServer();

            #region ����ApiBehaviorOptions��˵��
            /*
            MvcCoreServiceCollectionExtensions��
                       services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<ApiBehaviorOptions>, ApiBehaviorOptionsSetup>());
                       ��ʼ�� ApiBehaviorOptions�ӿڣ�ֱ��ʹ�� ApiBehaviorOptionsSetupʵ���࣬
                       �� ApiBehaviorOptionsSetup��ʵ������ʱ�򣬾�ֱ��д�� new BadRequestObjectResult
                       ���κ���ʹ�� ApiBehaviorOptionsSetup ʱ�����ʹ�õľ��� BadRequestBojectResult
                       ���� ��ModelStateInvalidFilterFactory��ʹ��ʱ��Ҫ IOptions<ApiBehaviorOptions>������ModelStateInvalidFilterȥʵ������
                   ModelStateInvalidFilter �и���·������
                   public void OnActionExecuting(ActionExecutingContext context)
                   {     
                       _apiBehaviorOptions ������һ�������ֵ
                       if (context.Result == null && !context.ModelState.IsValid)
                       {
                           _logger.ModelStateInvalidFilterExecuting();
                           context.Result = _apiBehaviorOptions.InvalidModelStateResponseFactory(context); //���ִ���� BadRequestObjectResult
                       }
                   }
            */
            #endregion

            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true); //�����Դ� ����֤

            services.AddSingleton<DynamicRoute>(); //�Զ���·���õ��ϵ�

            services.AddRouting(options => 
            {
                
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpContextAccessor accessor)
        {
            app.UseErrorHandling();//���������ʾ����
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

            #region ��׺����html�����ش���
            //app.Map("/.html", builder =>
            //{        // ����/�� �Ĵ���
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
                        await context.Response.WriteAsync("�ļ������ڣ�", System.Text.Encoding.UTF8);
                });
            });

            // ����·��ʽ����������ַ�����ƣ�������Ӧ���Ӹ��ӵ����
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
            //            //    streamWriter.Write("���token�Ѿ�������");
            //        });
            //    };
            //    router.MapMiddlewareRoute("{some}.html", must);
            //    router.MapMiddlewareRoute("{a?}/{some}.html", must);
            //});

            #endregion

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();//�ȿ�����֤
            app.UseAuthorization(); //�������Ȩ

            app.UseEndpoints(endpoints =>
            {
                // ��Ҫ����� ��services.AddSingleton<DynamicRoute>();
                //��д������� �Զ���·�ɣ�
                endpoints.MapDynamicControllerRoute<DynamicRoute>("{some}.ip");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
