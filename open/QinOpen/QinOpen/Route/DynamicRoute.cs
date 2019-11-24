using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace QinOpen
{
    /// <summary>
    /// NetCore3.0 的自定义路由。和NetCore3.0以下完全不一样，
    /// 用来拦截请求中，以 “.ip”结尾的，拦截路由，处理得到新的路由
    /// 指导博客：https://blog.csdn.net/qq_22949043/article/details/100548137
    /// </summary>
    public class DynamicRoute : DynamicRouteValueTransformer
    {
        public override async  ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
        {
            values["controller"] = "Home";
            values["action"] = "getIp";
            return   values;
        }
    }

}
