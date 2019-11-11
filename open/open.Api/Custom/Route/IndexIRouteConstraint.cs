
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace open.Api.Custom.Route
{
    /// <summary>
    /// 自定义路由约束
    /// </summary>
    public class IndexIRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {


            throw new NotImplementedException();
        }
    }
}
