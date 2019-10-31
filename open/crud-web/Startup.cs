using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using crud_base;
using crud_Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace crud_web
{
    public class Startup
    {
        public Startup(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public IConfiguration configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.InjectionBusiness(configuration); // 配置MySQL的链接
            services.InjectionBusinessServer(); //批量注入 服务类
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            /*=============================*/

            #region 这一种：[Authorize(AuthenticationSchemes = "myAuthentication")]   // myAuthentication--自己自定义的认证方案

            /*=================【授权】采用系统默认的就行====================*/


            /*=================【自定义认证】====================*/
            services.AddAuthenticationCore(options => options.AddScheme<MyHandler>("myAuthentication", "demo myAuthentication"));

            #endregion


            #region 第二种：[Authorize("Permission")] 或者 [Authorize(Roles = "admin1")] 

            //TODO： [Authorize("Permission")] 这个策略中，包含的很多个 Role角色 都可以访问这个控制器, Permission 是一个完全自定义的认证逻辑
            //TODO： [Authorize(Roles = "admin1") 。Jwt框架自带认证逻辑

            #region 【授权】

            //读取配置文件Permission
            IConfiguration audienceConfig = configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            // 角色与接口的权限要求参数
            PermissionRequirement permissionRequirement = new PermissionRequirement(
                "/api/denied", // 拒绝授权的跳转地址（目前无用）
                permissions: new List<PermissionItem>(),
                ClaimTypes.Role, //基于角色的授权
                audienceConfig["Issuer"], //发行人
                audienceConfig["Audience"], //听众
                new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256), //签名凭据
                expiration: TimeSpan.FromSeconds(60 * 2) //接口的过期时间
            );
            /*添加授权，配置授权的策略，控制器就这样写 [Authorize("Permission")] ，授权策略，他的角色在于 PermissionRequirement的 permissions#1*/
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Permission",
                    policy => policy.Requirements.Add(permissionRequirement)); //指定我们自定义的授权策略
            });
            // 注入 我们自定义的策略权限处理器，替换微软Core自带的 权限检查处理
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton(permissionRequirement);

            #endregion

            #region 【认证】Core 官方JWT认证

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKey,
                        ValidateIssuer = true,
                        ValidIssuer = audienceConfig["Issuer"], //发行人
                        ValidateAudience = true,
                        ValidAudience = audienceConfig["Audience"], //订阅人
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        RequireExpirationTime = true,
                    };
                    o.Events = new JwtBearerEvents
                    {
                        // Jwt框架在认证的过程中，自带4个事件，用于扩展
                        OnAuthenticationFailed = context =>
                        {
                            // 如果过期，则把<是否过期>添加到，返回头信息中
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }

                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            /*权限验证失败的事件*/
                            context.HandleResponse(); // 终止默认的返回类型和数据结果
                            context.Response.ContentType = "application/json";
                            context.Response.StatusCode = StatusCodes.Status200OK; //默认是401，改成200
                            var dde = Newtonsoft.Json.JsonConvert.SerializeObject(new { isok = false, Msg = "您没有权限！" });
                            context.Response.Body.Write(Encoding.UTF8.GetBytes(dde));
                            return Task.FromResult(0);
                        },
                        OnMessageReceived = context =>
                        {
                            /*收到消息*/
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            /*已经验证后*/
                            return Task.CompletedTask;
                        },
                    };
                });

            #endregion

            #endregion
        
        
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseAuthentication(); 

            //app.UseHttpsRedirection();
            app.UseCookiePolicy();

            app.UseMvc(iRouteBuilder =>
            {
                iRouteBuilder.MapRoute(
                    "aa",
                    "{controller=Default}/{action=JwtDemo}/{id?}"
                );
            });
            //app.UseMvcWithDefaultRoute();  //使用默认的路由
        }
    }
}