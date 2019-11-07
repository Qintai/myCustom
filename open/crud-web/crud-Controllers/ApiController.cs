using System;
using System.Net;
using System.Threading.Tasks;
using crud_entity;
using crud_server.Achieve;
using crud_server.connector;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace crud_Web.crud_Controllers
{
    /// <summary>
    /// RestFul风格--Chloe.Mysql半自动ORM的增删改查
    /// </summary>
    [ApiController]
    [Route("[Controller]")]
    [AllowAnonymous]
    [Produces("application/json")]
    public class ApiController : ControllerBase
    {
        #region Init
        private readonly IzUserSer izUserSer;

        public ApiController(IzUserSer _izUserSer)
        {
            izUserSer = _izUserSer;
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
        [Route("id={id:int}&name={name:string}")]
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

    }
}


