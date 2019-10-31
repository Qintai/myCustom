using System;
using System.Net;
using System.Threading.Tasks;
using crud_entity;
using crud_server.Achieve;
using crud_server.connector;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace crud_Web.crud_Controllers
{
    /// <summary>
    ///     这个控制器用来，展示Chloe.Mysql半自动ORM的增删改查
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class ApiController : ControllerBase
    {
        private readonly IzUserSer izUserSer;

        public ApiController(IzUserSer _izUserSer)
        {
            izUserSer = _izUserSer;
        }

        //Authorization
        [NonAction]
        private R GetServer<R>()
        {
            return (R) HttpContext.RequestServices.GetService(typeof(R));
        }

        [HttpGet]
        [Route("async/{id}")]
        // localhost:5001/api/async?id=1
        public async Task<string> GetzUser(int id)
        {
            var po = izUserSer[id];
            // Func<Task<TResult>> function
            // await Task.Run<zUser>(k =>
            //  {
            //    return new Task<zUser>(f => new zUser());
            //  },null);
            await new Task<string>(() => "耗时操作！");
            var user = izUserSer.GetModel(id);
            return JsonConvert.SerializeObject(user);
        }

        [HttpGet]
        // localhost:5001/api?id=1&dateTime=2019-08-08 12:11:11
        //  DateTime dateTime = DateTime.MinValue 不通过
        public async Task<string> GetzUser(int id, DateTime dateTime)
        {
            var isValid = ModelState.IsValid; //获取一个值，该值指示此模型状态中是否有任何模型状态值 字典无效或未验证。
            await new Task<string>(() => isValid.ToString());
            var po = izUserSer[id];
            var user = izUserSer.GetModel(id);
            return JsonConvert.SerializeObject(user);
        }

        [HttpPost]
        public zUser GetzUser(string name)
        {
            return izUserSer.GetModel(k => k.Name.Equals(name));
        }

        [HttpPut]
        public int GetAge18()
        {
            var count = izUserSer.AgeGreater18();
            return count;
        }


        [HttpGet]
        [Route("ww/{id}")]
        public zmyhork Gethome(int id)
        {
            var zmyhorkser = GetServer<zmyhorkSer>();
            var model = zmyhorkser[id];
            return model;
        }

        [HttpPost]
        [Route("ww/{id:int}")]
        [Authorize(Roles="topadmin")]
        public zmyhork Gethome1(int id)
        {
            var zmyhorkser = GetServer<zmyhorkSer>();
            var model = zmyhorkser[id];
            return model;
        }

        [HttpPost]
        public ActionResult setdata()
        {
            return new ViewResult();
        }
    }
}