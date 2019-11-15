using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace QinOpen.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class Route_ip_Middleware
    {
        private readonly RequestDelegate _next;

        public Route_ip_Middleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class SwaggerMiddlewareExtensions
    {
        public static IApplicationBuilder UseSwaggerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<Route_ip_Middleware>();
        }
    }
}
