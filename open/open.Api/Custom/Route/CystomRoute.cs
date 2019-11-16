using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace open.Api.Custom.Route
{
    //  自定义路由参考： https://www.cnblogs.com/shenba/p/6376245.html?utm_source=itdadao&utm_medium=referral
    public class CystomRoute : IRouter
    {
        private readonly IRouter _mvcRoute;
        private readonly IServiceProvider _services;

        /// <summary>
        /// 自定义路由，URL上有包含 “.ip”的访问，处理之后，交给MVC路由去处理
        /// 升级3.0 这个，报错了,3.0 没有这个 MvcRouteHandler
        /// </summary>
        /// <param name="services"></param>
        public CystomRoute(IServiceProvider services)
        {
            _services = services;
            // RouteHandler

            //_mvcRoute = services.GetRequiredService<MvcRouteHandler>(); //升级3.0 这个，报错了,3.0 没有这个 MvcRouteHandler

        }

        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            return null;
        }

        public async Task RouteAsync(RouteContext context)
        {
            if (context.HttpContext.Request.Path.Value.Contains(".ip"))
            {
                /*
                      //[ApiController]
                      //[Route("[controller]/[action]")]
                       使用这一种方式，控制器上不能有  ApiController 和 Route的特性，否则没有效果
                 */

                context.RouteData.Values["action"] = "getIp";
                context.RouteData.Values["controller"] = "Default";
                await _mvcRoute.RouteAsync(context); // 使用MVCHander，尝试处理这个MVC路由模式的请求

                //这样使用 Add 也是可以的
                //context.RouteData.Values.Add("action", "getIp"); //不成功
                //context.RouteData.Values.Add("controller", "Default"); //不成功
                //await _mvcRoute.RouteAsync(context);
            }
            //return Task.CompletedTask;
        }
    }
}
