using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QinEntity;
using System.Collections.Generic;

namespace QinOpen.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Produces("application/json")]
    public class ProjectUserController : Controller
    {
        MessageModel _msg;
        public ProjectUserController(MessageModel msg)
        {
            _msg = msg;
        }

        [Route("login")]
        [HttpGet("Login")]
        public MessageModel Login(string name ,string pwd)
        {
            return _msg;
        }

        /// <summary>
        /// 用户列表--C级权限访问
        /// </summary>
        /// <returns></returns>
        [Route("GetUserList")]
        [HttpGet("Login")]
        [Authorize(Roles ="C")]
        public MessageModel GetUserList()
        {
            _msg.data = new List<zUser>();
            return _msg;
        }

    }
}