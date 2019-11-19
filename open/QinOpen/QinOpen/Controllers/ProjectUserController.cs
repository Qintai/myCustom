using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QinCommon.Common.Helper;
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
        zCustomUserRolesService _roleserver;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="userver"></param>
        public ProjectUserController(
            MessageModel msg,
            zCustomUserService userver,
            zCustomUserRolesService roleserver
            )
        {
            _msg = msg;
            _userver = userver;
            _roleserver = roleserver;
        }

        /// <summary>
        /// 登陆接口
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        [Route("login")]
        [HttpGet]
        public async Task<MessageModel> Login(string Name, string pwd)
        {
            var model = await _userver.GetModelAsync(m => m.Name.Equals(Name));
            if (model == null)
            {
                _msg.msg = "没有找到用户！";
                _msg.success = false;
                return _msg;
            }
            if (model.pwd != MD5Helper.MD5Encrypt32(pwd))
            {
                _msg.msg = "用户密码输入错误！";
                _msg.success = false;
                return _msg;
            }
            var rolem = await _roleserver.GetModelAsync(a => a.userId == model.Id);
            if (rolem != null)
            {
                TokenModelJwt tokenModel = new TokenModelJwt { Uid = model.Id, Role = rolem.RoleName };
                string jwtStr = JwtHelper.IssueJwt(tokenModel);
                _msg.data = jwtStr;
            }
            _msg.msg = "请求成功！";
            _msg.success = true;
            return _msg;
        }

        /// <summary>
        /// 当前用户
        /// https://localhost:5001/api/user??Name=h&pwd=13 , 前端没有传递ContentType，使用FromQuery可以绑定实体，
        /// 不支持 https://localhost:5001/api/user?{"Name":"h","pwd":"123 "} ，不支持这一种
        /// </summary>
        /// <param name="viewmodel"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageModel GetCustom([FromQuery]ViewModel viewmodel)
        {
            _msg.code = ModelState.IsValid.ToString();
            _msg.msg = $"你输入的Name={viewmodel.Name}，pwd={viewmodel.pwd}";
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