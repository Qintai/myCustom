using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crud.Web.Custom.Route
{
    public class IndexRouteBuilder : IRouteBuilder
    {
        public IApplicationBuilder ApplicationBuilder => throw new NotImplementedException();

        public IRouter DefaultHandler { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IServiceProvider ServiceProvider => throw new NotImplementedException();

        public IList<IRouter> Routes => throw new NotImplementedException();

        public IRouter Build()
        {
            throw new NotImplementedException();
        }
    }
}
