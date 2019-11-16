using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace open.Api.Middleare
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class _404Middleware
    {
        private readonly RequestDelegate _next;

        public _404Middleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class _404MiddlewareExtensions
    {
        public static IApplicationBuilder Use_404Middleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<_404Middleware>();
        }
    }
}
