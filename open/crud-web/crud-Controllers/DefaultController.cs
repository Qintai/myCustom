using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace crud_web
{
    /// <summary>
    ///     这个控制器，用来展示授权的
    /// </summary>
    public class DefaultController : Controller
    {
        private  readonly ILoggerFactory _Factory;
        private  readonly ILogger<DefaultController> _logger;
        private readonly IConfiguration configuration;
        
        public DefaultController(
            ILoggerFactory factory, 
            ILogger<DefaultController> logger,
            IConfiguration _configuration )
        {
            // TODO:log4net.ILog log是不能被注入的
            configuration = _configuration;
            _Factory = factory;
            _logger = logger;

            //   this._Factory.CreateLogger<DefaultController>().LogError("这里是ILoggerFactory Error"); 
            //   this._logger.LogError("这里是ILogger<DefaultController> Error");
            //   this._logger.LogDebug($"User Name { base.HttpContext.User?.Identity?.Name}");
            //   this._logger.LogDebug($"fff");
        }

        #region 使用自带ValidateAntiForgeryToken

        public ActionResult Index()
        {
            /* */
            return View();
        }

        // [AcceptVerbs(HttpVerbs.Post)] netCore没有这个
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Index(IFormCollection collection)
        {
            // IHtmlContent kk= new HtmlHelper().AntiForgeryToken();
            //IHtmlGenerator
            //Microsoft.AspNetCore.Mvc.TagHelpers.TestableHtmlGenerator
            //HtmlGenerator.GenerateAntiforgery();

            ModelState.AddModelError("", "1111111111111");
            return Json("好的");
        }

        #endregion


        public ActionResult JwtDemo()
        {
            return View();
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetJwt(string name = "", string pass = "")
        {
            string jwtStr = string.Empty;
            bool suc = false;
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString());
            await  Task.Run(()=> 
            {
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString() );
                suc = string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pass);
            });
            if (suc)
            {
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString());
                AjaxResult result = new AjaxResult();
                result.msg = "用户名或密码不能为空";
                result.isok = false;
                return Content("验证通过");
                //return result;
            }

            PermissionRequirement _requirement =(PermissionRequirement) HttpContext.RequestServices.GetService(typeof(PermissionRequirement));
            var userRoles = "Admin";
            //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
            var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(JwtRegisteredClaimNames.Jti, "1"), //假设当前的用户ID=1
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_requirement.Expiration.TotalSeconds).ToString()) };
            claims.AddRange(userRoles.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));
            //用户标识
            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
            identity.AddClaims(claims);
            var token = JwtToken.BuildJwtToken(claims.ToArray(), _requirement);
            return new JsonResult(token);
        }

        [Authorize(AuthenticationSchemes = "myAuthentication")] // myAuthentication,自己自定义的认证方案
        public ActionResult lk1()
        {
            //调用，想拿到当前登陆人的一些信息，比如登陆人的Id，Name啊
             Claim userid = User.Claims.FirstOrDefault(c => c.Type == System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti);
             string curruserId = userid?.Value; //这样就拿到当前登陆人的Uid   

             Claim nname = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name); //为什么是  ClaimTypes.Name，取决于你写进Claim时的 用的type值
             string sname = nname?.Value;  //这样就拿到当前登陆人的Name  
             return Content("验证通过");
        }


        [Authorize("Permission")]  // 这个策略中，包含的很多个 Role角色 都可以访问这个控制器, Permission 是一个完全自定义的认证逻辑
        public ActionResult lk2()
        {
        

             return Content("验证通过lk2");
        }



        [Authorize(Roles ="Guse" )]  //  系统框架的认证逻辑
        public ActionResult lk3()
        {

             return new CustomResult("验证通过lk3");
        }




    }
}



