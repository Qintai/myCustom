
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Cms;

namespace crud_web
{
    /// <summary>
    /// 自定义策略：权限授权处理器
    /// </summary>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <summary>
        /// 验证方案提供对象
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }

        private ILogger<PermissionHandler> _logger; // 注入了日志

        /// <summary>
        /// 构造函数注入 
        /// </summary>
        /// <param name="schemes"></param>
        /// <param name="roleModulePermissionServices"></param>
        public PermissionHandler(IAuthenticationSchemeProvider schemes, ILogger<PermissionHandler> logger)
        {
            Schemes = schemes;
            _logger = logger;
        }

        // 重载异步处理程序
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            requirement.Permissions = new List<PermissionItem>() {
                new PermissionItem (){Role="Admin",Url="/api/values/get" },
                new PermissionItem (){Role="Admin",Url="/api/values/post" },
                new PermissionItem (){Role="Admin",Url="/api/values/put" },
                new PermissionItem (){Role="Admin",Url="/api/values/delete" },
            };

            //从AuthorizationHandlerContext转成HttpContext，以便取出表求信息
            var httpContext = (context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext).HttpContext;
            //请求Url
            var questUrl = httpContext.Request.Path.Value.ToLower();
            //判断请求是否停止
            var handlers = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            foreach (var scheme in await Schemes.GetRequestHandlerSchemesAsync())
            {
                var handler = await handlers.GetHandlerAsync(httpContext, scheme.Name) as IAuthenticationRequestHandler;
                if (handler != null && await handler.HandleRequestAsync())
                {
                    context.Fail();
                    return;
                }
            }
            //判断请求是否拥有凭据，即有没有登录
            var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenticate != null)
            {
                var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                //result?.Principal不为空即登录成功
                if (result?.Principal != null)
                {
                    httpContext.User = result.Principal;


                    var isMatchUrl = false; //是否匹配URL

                    var permisssionGroup = requirement.Permissions.GroupBy(g => g.Url);
                    foreach (var item in permisssionGroup)
                    {
                        try
                        {
                            if (Regex.Match(questUrl, item.Key?.ToString().Trim().ToLower())?.Value == questUrl)
                            {
                                isMatchUrl = true; //URL匹配成功
                                break;
                            }
                        }
                        catch (Exception)
                        { }
                    }


                    //权限中是否存在请求的url
                    //if (requirement.Permissions.GroupBy(g => g.Url).Where(w => w.Key?.ToLower() == questUrl).Count() > 0)
                    if (isMatchUrl)
                    {
                        // 获取当前用户的角色信息,上下文的 Claims 已经被框架解析token得到结果并且赋值给Claims，这里直接就可以使用了
                        var currentUserRoles = (from item in httpContext.User.Claims
                                                where item.Type == requirement.ClaimType
                                                select item.Value).ToList();

                        var isMatchRole = false; //是否角色匹配
                        var permisssionRoles = requirement.Permissions.Where(w => currentUserRoles.Contains(w.Role));
                        foreach (var item in permisssionRoles)
                        {
                            try
                            {
                                if (Regex.Match(questUrl, item.Url?.ToString().Trim().ToLower())?.Value == questUrl)
                                {
                                    isMatchRole = true;
                                    break;
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }

                        //验证权限
                        //if (currentUserRoles.Count <= 0 || requirement.Permissions.Where(w => currentUserRoles.Contains(w.Role) && w.Url.ToLower() == questUrl).Count() <= 0)
                        if (currentUserRoles.Count <= 0 || !isMatchRole)
                        {

                            context.Fail();
                            return;
                            // 可以在这里设置跳转页面，不过还是会访问当前接口地址的
                            httpContext.Response.Redirect(requirement.DeniedAction);
                        }
                    }
                    else
                    {
                        context.Fail();
                        return;

                    }
                    //判断过期时间
                    if ((httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value) != null && DateTime.Parse(httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value) >= DateTime.Now)
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                        return;
                    }
                    return;
                }
                else
                {
                    //有凭据但是，认证失败
                    WriteByString(httpContext, false, "认证失败！");
                    return;
                }
            }
            else
            {
                if (questUrl == requirement.LoginPath.ToLower())
                {
                    if (httpContext.Request.Method.Equals("POST") || httpContext.Request.HasFormContentType)
                    {
                        WriteByString(httpContext, false, $"您没有登录！，请到地址去登录 {requirement.LoginPath.ToLower()}");//context.Fail();
                        return;
                    }
                    else
                    {
                        httpContext.Response.Redirect(requirement.DeniedAction); // 非Ajax请求，跳转到登陆页面
                        return;
                    }
                }

                      //判断没有登录时，是否访问登录的url,并且是Post请求，并且是form表单提交类型，否则为失败#1#
                //if (!questUrl.Equals(requirement.LoginPath.ToLower(), StringComparison.Ordinal) && (!httpContext.Request.Method.Equals("POST")
                //|| !httpContext.Request.HasFormContentType))
                //{
                //    httpContext.Response.Redirect(requirement.DeniedAction);//context.Fail();
                //    return;
                //}                
            }
            context.Succeed(requirement);
        }
    
        /// <summary>
        /// 输出文本
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="state"></param>
        /// <param name="str"></param>
        public  void WriteByString(HttpContext httpContext,bool state,string str)
        {
            httpContext.Response.Headers.Add("Content-type", "application/json");
            using (StreamWriter sw = new StreamWriter(httpContext.Response.Body, Encoding.UTF8))
            {
                var viewresult = new {isok = state, msg = str};
                var jsonString= Newtonsoft.Json.JsonConvert.SerializeObject(viewresult);
                sw.Write(jsonString);
            }
        }

    }
}

