using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QinEntity;
using QinServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QinOpen.Controllers
{
    /// <summary>
    /// 用户操作
    /// </summary>
    [ApiController]
    [Route("api/user")]
    [Produces("application/json")]
    public class ProjectUserController : Controller
    {
        MessageModel _msg;
        zCustomUserService _userver;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="userver"></param>
        public ProjectUserController(MessageModel msg, zCustomUserService userver)
        {
            _msg = msg;
            _userver = userver;
        }

        /// <summary>
        /// 登陆接口
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        [Route("login")]
        [HttpGet]
        public async Task<MessageModel> Login(string name, string pwd)
        {
            var mj = await _userver.GetModelAsync(m => m.Name.Equals(name));
            _msg.data = mj;
            _msg.msg = "请求成功！";
            _msg = true;
            return _msg;
        }

        /// <summary>
        /// 用户列表--C级权限访问
        /// </summary>
        /// <returns></returns>
        [Route("GetUserList")]
        [HttpGet]
        [Authorize(Roles = "C")]
        public MessageModel GetUserList()
        {
            _msg.data = new List<zUser>();
            return _msg;
        }



    }
}