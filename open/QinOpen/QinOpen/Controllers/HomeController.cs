﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            // services.Configure<InterExample>(configuration.GetSection("InterExample"));  //配置为 InterExample 注入对象成功
            //InterExample interExample = (InterExample)HttpContext.RequestServices.GetService(typeof(InterExample));
     
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
    }
}
