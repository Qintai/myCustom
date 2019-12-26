using Blog.Core.AuthHelper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using QinCommon;
using QinCommon.Common;
using QinCommon.Common.AppConfig;
using QinEntity;
using QinOpen.AuthHelper;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QinOpen
{
    public static class JwtAuthorization
    {

        /// <summary>
        /// 添加JWT授权，单个角色，单个策略
        /// </summary>
        /// <param name="services"></param>
        public static void Jwt(this IServiceCollection services)
        {
            //读取配置文件
            var symmetricKeyAsBase64 = AppSecretConfig.Audience_Secret_String;
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var Issuer = Appsettings.app(new string[] { "AppSettings", "Audience", "Issuer" });
            var Audience = Appsettings.app(new string[] { "AppSettings", "Audience", "Audience" });
            //  var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            //一组授权，组名：EveoneAdmin，包含权限：admin_a,admin_b,admin_c
            services.AddAuthorization(k =>
            {
                k.AddPolicy(RoleHelper.EveoneAdmin, p => p.RequireRole(
                    RoleHelper.Roletype.admin_a.ToString(),
                    RoleHelper.Roletype.admin_b.ToString(),
                    RoleHelper.Roletype.admin_c.ToString()
                   ).Build());
            });

            services.AddAuthentication(f =>
            {
                f.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                // f.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //身份验证认证的方案
                // f.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // 身份验证（授权）方案 Bearer
            }).AddJwtBearer(jwt =>
            {
                jwt.RequireHttpsMetadata = false;// 获取或设置元数据地址或权限是否需要https。这个  默认值为true。这应该只在开发环境中禁用。
                jwt.SaveToken = true; //定义承载令牌是否应存储在Microsoft.aspnetcore.http.authentication.authenticationproperties中   在成功授权之后。

                // TokenValidationParameters：获取或设置用于验证标识令牌的参数。
                jwt.TokenValidationParameters = new TokenValidationParameters()
                {
                    //获取或设置表示将使用的有效颁发者的System.String。  检查令牌的颁发者。
                    ValidIssuer = JwtParameter.ValidIssuer,
                    //获取或设置一个字符串，该字符串表示将用于检查的有效访问群体。  反对代币的观众。
                    ValidAudience = JwtParameter.ValidIssuer,
                    // 获取或设置要使用的Microsoft.IdentityModel.Tokens.SecurityKey。用于签名验证。
                    IssuerSigningKey = JwtParameter.IssuerSigningKey,
                    ClockSkew = TimeSpan.FromSeconds(30),
                };

                #region MyRegion
                //jwt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                //{
                //    ValidateIssuerSigningKey = true,
                //    IssuerSigningKey = signingKey,
                //    ValidateIssuer = true,
                //    ValidIssuer = Issuer,//发行人
                //    ValidateAudience = true,
                //    ValidAudience = Audience,//订阅人
                //    ValidateLifetime = true,
                //    ClockSkew = TimeSpan.FromSeconds(30),
                //    RequireExpirationTime = true,
                //};
                #endregion


                jwt.Events = new JwtBearerEvents()
                {
                    OnForbidden = f =>
                    {
                        string b = "";
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = f =>
                    {
                        string b = "";
                        return Task.CompletedTask;
                    },
                    OnChallenge = f =>
                    {
                        string b = "";
                        return Task.CompletedTask;
                    },
                    OnMessageReceived = f =>
                   {
                        // result.Principal：获取具有已验证用户身份的声明主体
                        if (f.Principal == null) //结果都是 null
                       {

                       }

                       return Task.CompletedTask;
                   }
                };
            });

        }

        /// <summary>
        ///  自定义的一组授权策略,组名  Permission【Permissions.Name】，组内角色：自定义
        /// </summary>
        /// <param name="services"></param>
        public static void AddCustomAuthorization(this IServiceCollection services)
        {
            //读取配置文件
            var symmetricKeyAsBase64 = AppSecretConfig.Audience_Secret_String;
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var Issuer = Appsettings.app(new string[] { "Audience", "Issuer" });
            var Audience = Appsettings.app(new string[] { "Audience", "Audience" });
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // 如果要数据库动态绑定，这里先留个空，后边处理器里动态赋值
            var permission = new List<PermissionItem>();

            // 角色与接口的权限要求参数
            PermissionRequirement permissionRequirement = new PermissionRequirement(
                   deniedAction: "/api/denied",// 拒绝授权的跳转地址（目前无用）
                   permissions: permission,
                 claimType: ClaimTypes.Role,//基于角色的授权
                   issuer: Issuer,//发行人
                 audience: Audience,//听众
                 signingCredentials: signingCredentials,//签名凭据
                 expiration: TimeSpan.FromSeconds(60 * 60)//接口的过期时间
            );


            services.AddAuthorization(options =>
            {
                options.AddPolicy(Permissions.Name,
                         policy => policy.Requirements.Add(permissionRequirement));
            });

            // 替换默认的授权过滤器权限处理器,先把 permissionRequirement 注册单例
            services.AddSingleton(permissionRequirement);
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
        }

    }
}
