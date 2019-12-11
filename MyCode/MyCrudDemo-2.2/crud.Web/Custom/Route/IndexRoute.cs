using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace crud.Web.Custom.Route
{
    /// <summary>
    /// 自定义路由
    /// https://www.cnblogs.com/shenba/p/6376245.html?utm_source=itdadao&utm_medium=referral
    /// </summary>
    public class IndexRoute : IRouter
    {
        private readonly string[] _urls;

        private readonly IRouter _mvcRoute;

        public IndexRoute(IServiceProvider services, params string[] urls)
        {
            _urls = urls;
              // Microsoft.AspNetCore.Routing.RouteHandler
            _mvcRoute = services.GetRequiredService<MvcRouteHandler>();
        }

        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            if (context.Values.ContainsKey("legacyUrl"))
            {
                var url = context.Values["legacyUrl"] as string;
                if (_urls.Contains(url))
                {
                    return new VirtualPathData(this, url);
                }
            }
            return null;
        }

        public async Task RouteAsync(RouteContext context)
        {
            if (context.HttpContext.Request.Path.Value.Contains("html"))
            {
                context.RouteData.Values["action"] = "get";
                context.RouteData.Values["controller"] = "P";
                await _mvcRoute.RouteAsync(context);

                //context.RouteData.Values.Add("action", "get"); //不成功
                //context.RouteData.Values.Add("controller", "p"); //不成功
                //await _mvcRoute.RouteAsync(context);
            }

            //var requestedUrl = context.HttpContext.Request.Path.Value.TrimEnd('/');
            //if (_urls.Contains(requestedUrl, StringComparer.OrdinalIgnoreCase))
            //{
            //    context.Handler = async ctx =>
            //    {
            //        var response = ctx.Response;
            //        byte[] bytes = Encoding.ASCII.GetBytes($"This URL: {requestedUrl} is not available now");
            //        await response.Body.WriteAsync(bytes, 0, bytes.Length);
            //    };
            //}
            //return Task.CompletedTask;
        }
    }
}
