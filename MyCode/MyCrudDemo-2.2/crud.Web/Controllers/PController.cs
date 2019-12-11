using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using crud.Web.Custom.Filter;
using crud.Web.Filter;

namespace crud.Web.Controllers
{
    /// <summary>
    ///  JsonP
    /// </summary>
    [ApiController]
    [Route("[Controller]")]
    [ActionControllerActionFilter]
    public class PController : ControllerBase
    {
        [Route("M")]
        [HttpGet]
        public string get()
        {
            /* 
             *  $.ajax({
             *         type: "get",
             *         async: false,
             *         url: "http://localhost:5000/p/m",
             *         dataType: "jsonp",
             *          // jsonp: "a",//传递给请求处理程序或页面的，用以获得jsonp回调函数名的参数名(一般默认为:callback)
             *         //  jsonpCallback: "KM",//自定义的jsonp回调函数名称，默认为jQuery自动生成的随机函数名，也可以写"?"，jQuery会自动为你处理数据
             *         success: function(json) {
             *                 console.info(json);
             *          },
             *         error: function() {}
             *  });
             */

            return "{\"ip\":\"220.181.38.150\",\"dz\":\"北京市\",\"wl\":\"电信互联网数据中心\"}";
        }
    }
}


