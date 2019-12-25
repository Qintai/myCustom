using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using QinCommon;
using QinOpen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Blog.Core.AuthHelper
{

    /// <summary>
    /// 权限授权处理器，
    /// </summary>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <summary>
        /// 验证方案提供对象
        /// </summary>
        public IAuthenticationSchemeProvider _Schemes { get; set; }

        /// <summary>
        /// 为了获取HttpContext
        /// </summary>
        private readonly IHttpContextAccessor _accessor;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="schemes"> 验证方案提供对象</param>
        /// <param name="accessor">上下文</param>
        public PermissionHandler(IAuthenticationSchemeProvider schemes, IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            _Schemes = schemes;
        }

        // 重写异步处理程序
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            /*
             * 
             * 首先必须在 controller 上进行配置 Authorize ，可以策略授权，也可以角色等基本授权
             * 
             * 1、开启公约， startup 中的全局授权过滤公约：o.Conventions.Insert(0, new GlobalRouteAuthorizeConvention());
             * 
             * 2、不开启公约，使用 IHttpContextAccessor ，也能实现效果；
             */

            //var filterContext = (context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext);  //从AuthorizationHandlerContext转成HttpContext，以便取出表求信息
            var httpContext = (context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext)?.HttpContext;

            if (httpContext == null)
                httpContext = _accessor.HttpContext;
            if (httpContext == null)
            {
                context.Fail();
                return;
            }
            MessageModel vresult = httpContext.RequestServices.GetService<MessageModel>();

            #region 判断请求是否停止
            var handlers = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            foreach (var scheme in await _Schemes.GetRequestHandlerSchemesAsync())
            {
                var aher = await handlers.GetHandlerAsync(httpContext, scheme.Name);
                if (aher is IAuthenticationRequestHandler handler && await handler.HandleRequestAsync())
                {
                    vresult.Msg = "请求已经停止";
                    await httpContext.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(vresult));
                    return;
                }
            }
            #endregion

            #region 判断请求是否拥有凭据，即有没有登录
            var defaultAuthenticate = await _Schemes.GetDefaultAuthenticateSchemeAsync();//返回默认情况下将用于微软的方案
            if (defaultAuthenticate == null)
            {

                if (vresult == null)
                    vresult = new MessageModel();
                var questUrl = httpContext.Request.Headers["referer"];
                vresult.Msg = "没有登录";
                vresult.Code = questUrl.ToString();
                await httpContext.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(vresult));
                return;
            }
            #endregion

            #region 基础认证的判断
            AuthenticateResult result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
            // result.Principal：获取具有已验证用户身份的声明主体
            if (result == null || result.Principal == null)
            {
                vresult.Msg = "认证失败";
                await httpContext.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(vresult));
            }
            #endregion

            httpContext.User = result.Principal;
            // 获取当前用户的角色信息
            //  List<string> currentUserRoles = httpContext.User.Claims.Where(a => a.Type == requirement.ClaimType).Select(b=>b.Value).ToList();
            List<string> currentUserRoles = (from item in httpContext.User.Claims
                                             where item.Type == requirement.ClaimType
                                             select item.Value).ToList();

            if (currentUserRoles.Count <= 0)
            {
                vresult.Msg = "认证失败,用户信息错误";
                await httpContext.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(vresult));
            }

            var isMatchRole = false;
            var permisssionRoles = requirement.Permissions.Where(w => currentUserRoles.Contains(w.Role));

            #region 判断你设定众多个的角色，迭代多角色时，判断有没有一个角色  在当登录用户中存在 并且 角色的url与请求匹配
            foreach (var item in permisssionRoles)
            {
                try
                {
                    var questUrl = httpContext.Request.Path.Value.ToLower();
                    //if (Regex.Match(questUrl, item.Url?.ObjToString().ToLower())?.Value == questUrl)
                    if (requirement.Permissions.Any(w => currentUserRoles.Contains(w.Role) && w.Url.ToLower() == questUrl))
                    {
                        isMatchRole = true;
                        break;
                    }
                }
                catch (Exception)
                { }
            }

            if (!isMatchRole)
            {
                vresult.Msg = "认证失败";
                await httpContext.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(vresult));
            }
            #endregion


            //判断过期时间（这里仅仅是最坏验证原则，你可以不要这个if else的判断，因为我们使用的官方验证，Token过期后上边的result?.Principal 就为 null 了，进不到这里了，因此这里其实可以不用验证过期时间，只是做最后严谨判断）
            var a = httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value != null;
            var b = DateTime.Parse(httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value) >= DateTime.Now;
            var flag = a && b;
            if (!flag)
            {
                vresult.Msg = "认证失败,用户信息已经过期";
                await httpContext.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(vresult));
                return;
            }
            context.Succeed(requirement);
        }

    }
}