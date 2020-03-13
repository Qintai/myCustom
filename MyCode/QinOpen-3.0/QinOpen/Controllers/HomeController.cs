using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QinCommon;
using QinCommon.Redis.Exchange.Service;
using QinEntity;
using QinOpen.Models;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace QinOpen.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// 字符串形式的Redis
        /// </summary>
        RedisStringService _redisStringService;

        /// <summary>
        /// Hash结构的
        /// </summary>
        RedisHashService _redisHashService;

        public HomeController(ILogger<HomeController> logger, Microsoft.AspNetCore.Mvc.Infrastructure.IClientErrorFactory clientError, RedisStringService redisStringService, RedisHashService redisHashService)
        {
            _logger = logger;
            _redisStringService = redisStringService;
            _redisHashService = redisHashService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            zCustomUser uu = new zCustomUser() { Gender = 0, Name = "fd" };
            _redisHashService.HashSet("mm", "pp0", uu);
            _redisHashService.HashSet("mm", "pp1", new zCustomUser() { Gender = 1, Name = "官方公告" });

            _redisStringService.StringSet("aa", uu);
            string val = _redisStringService.StringGet("aa");
            zCustomUser getuu = _redisHashService.HashGet<zCustomUser>("mm", "pp");

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
            /*
             [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            这样就等同于在，Reshponse的头部加入 Cache-Control 
            ResponseCacheLocation.None= no-cache
            ResponseCacheLocation.Client= private
            ResponseCacheLocation.Any= public
             */
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

        [HttpGet]
        public void swagger()
        {
            base.Redirect("/swagger/index.html");
        }


        [HttpGet]
        public async Task<string> PO()
        {
            _logger.LogDebug("上面" + Thread.CurrentThread.ManagedThreadId.ToString());

            return await Task.Run(() =>
             {
                 _logger.LogDebug("里面" + Thread.CurrentThread.ManagedThreadId.ToString());

                 return "1";
             });
            _logger.LogDebug("下面" + Thread.CurrentThread.ManagedThreadId.ToString());

            //  "{\"ip\":\"220.181.38.150\",\"dz\":\"北京市\",\"wl\":\"电信互联网数据中心\"}";
        }

    }
}
