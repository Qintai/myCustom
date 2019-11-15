using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using open.Api.Custom.Filter;
using open.Api.Filter;

namespace open.Api.Controllers
{
    /// <summary>
    ///  JsonP
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    [ActionControllerActionFilter]
    public class PController : ControllerBase
    {
        /// <summary>
        /// 获取经纬度
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Longitudeandlatitude()
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

            return "{\"status\":1,\"exact\":2,\"gps\":false,\"gcj\":{\"lng\":113.3292238,\"lat\":23.1361035},\"bd09\":{\"lng\":113.3357638,\"lat\":23.1418124},\"url\":\"https://cdn.asilu.com/map/#lng=113.3292238&lat=23.1361035\",\"address\":\"广东省广州市天河区林和街道财富广场羊城国际商贸中心\",\"ip\":\"113.111.9.158\",\"cache\":1}";
        }
    }
}


