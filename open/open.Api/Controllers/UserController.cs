using crud_entity;
using crud_server.connector;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace open.Api.Controllers
{
    /// <summary>
    /// RestFul风格--Chloe.Mysql半自动ORM的增删改查
    /// </summary>
    [ApiController]
    [Route("[Controller]")]
    [AllowAnonymous]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        #region Init
        private readonly IzUserSer _izUserSer;
        private readonly IzunderlyingSer _zunderingser;
        private readonly AjaxResult _ajaxResult;

        public UserController(IzUserSer _izUserSer, IzunderlyingSer zunderingser,AjaxResult ajaxResult)
        {
            this._izUserSer = _izUserSer;
            this._zunderingser = zunderingser;
            this._ajaxResult = ajaxResult;

        }
        //Authorization
        [NonAction]
        private R GetServer<R>()
        {
            return (R)HttpContext.RequestServices.GetService(typeof(R));
        }
        #endregion

        #region GET
        [HttpGet]
        public List<zUser> User0()
        {
            return new List<zUser>() { new zUser() { Id = 2, Name = "h" }, new zUser() { Id = 23, Name = "pp" } };
            /*var list = izUserSer.GetList();
            return list;*/
        }

        [HttpGet]
        [Route("{id:int}")]
        // http://localhost:55429/api/43
        public zUser User1(int id)
        {
            return new zUser() { Id = id, Name = "rr" };
            /*var model = izUserSer[id];
            var user = izUserSer.GetModel(id);
            return model;*/
        }

        [HttpGet]
        [Route("{id}&{dateTime}")]
        // http://localhost:55429/api/43&aaaa
        public zUser User3(int id, string dateTime)
        {
            return new zUser() { Id = 55, Name = "rr", Age = 777 };
        }


        [Route("age={age:int}")]
        // http://localhost:55429/Api/age=43
        [HttpGet]
        public zUser User2(int age)
        {
            return new zUser() { Id = 32, Name = "年龄", Age = age };
        }
        #endregion

        #region Update
        [HttpPut]
        [Route("id={id:int}&name={name}")]
        public string update1(int id,string name)
        {
            //zUser mode = GetServer<zUserSer>().GetModel(id);
            //mode.Name = name;
            return $"id={id}的实体更新成功！";
        }
        #endregion

        #region DELETE
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]
        public string delete(int id)
        {

            return "删除成功！";
        }
        #endregion

        #region ADD
        [HttpPost]
        [Route("{id:int}")]
        public string add(int id)
        {
            return "添加成功！";
        }
        #endregion

        [HttpPost]
        [Route("login")]
        public AjaxResult Login([FromQuery]string a,[FromQuery]string b) 
        {
            var model= _zunderingser.GetModel(v=>v.Nickname.Equals(a));
            if (model == null)
            {
                _ajaxResult.isok = false;
                _ajaxResult.msg = "没有找到该用户！";
                return _ajaxResult; 
            }
            model = _zunderingser.GetModel(v => v.Nickname.Equals(a) && v.pwd.Equals(b));
            if (model.Role == Permissions.Stop.ToString())
            {
                _ajaxResult.msg = "用户已经被禁止！";
                return _ajaxResult;
            }
            TokenModelJwt modelJwt = new TokenModelJwt();
            modelJwt.Role = model.Role;
            modelJwt.Uid = model.GuId.ToString();
            string token= JwtHelper.IssueJwt(modelJwt);

            _ajaxResult.code = token;
            _ajaxResult.isok = true;
            return _ajaxResult;
        }

    }
}


