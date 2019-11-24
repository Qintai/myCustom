using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QinCommon;
using QinOpen.Models;
using System.Diagnostics;

namespace QinOpen.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger, Microsoft.AspNetCore.Mvc.Infrastructure.IClientErrorFactory clientError)
        {

            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Log4helper<HomeController>.Info($"进入了{nameof(HomeController)} 的 {nameof(Index)}");
            // services.Configure<InterExample>(configuration.GetSection("InterExample"));  //配置为 InterExample 注入对象成功,可以获取对象信息
            // InterExample interExample = (InterExample)HttpContext.RequestServices.GetService(typeof(InterExample));
            _logger.LogError("控制台显示");
            return View();
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// 拦截路由，后跳转到此
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string getIp()
        {
            return "{\"ip\":\"220.181.38.150\",\"dz\":\"北京市\",\"wl\":\"电信互联网数据中心\"}";
        }


        /// <summary>
        /// JsonDemo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Produces("application/html")]
        public string JsonDemo()
        {
            return "{\"addr\":\"内蒙古自治区 锡林郭勒盟 锡林浩特市\",\"date\":\"1994-05-14\",\"sex\":\"女\",\"ckeck\":true}";
        }

    }
}
