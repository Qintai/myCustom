using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinCommon.HttpContextUser
{
    public class HttpContextHelper
    {
        public static IHttpContextAccessor Accessor;

        public static HttpContext hc { get=> Accessor.HttpContext;  }
    }
}
