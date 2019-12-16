using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QinCommon.Common.Helper;
using QinEntity;
using QinOpen.AuthHelper;
using IQinServices;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace QinOpen.Controllers
{
    /// <summary>
    /// 用户操作
    /// </summary>
    [ApiController]
    [Route("api/user/[action]")]
    [Produces("application/json")]
    public class ProjectUserController : Controller
    {
        MessageModel _msg;
        IzCustomUserService _userver;
        IzCustomUserRolesService _roleserver;

        public ProjectUserController(
            MessageModel msg,
            IzCustomUserService userver,
            IzCustomUserRolesService roleserver
            )
        {
            _msg = msg;
            _userver = userver;
            _roleserver = roleserver;
        }

        /// <summary>
        /// 登陆接口
        /// </summary>
        /// <param name="Name">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        [Route("{Name}/{pwd}")]
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
                string jwtStr = "";
                #region 封装好了的，没有测试成功
                //TokenModelJwt tokenModel = new TokenModelJwt
                //{
                //    Uid = model.Id,
                //    Role = rolem.RoleName
                //};
                // jwtStr = JwtHelper.IssueJwt(tokenModel);
                #endregion

                #region 写入证件信息
                // 1.写好这个证件中，有哪些信息，一个Claim 对象代表着一个证件中某一个信息
                Claim[] claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, model.Name), //证件用户名
                    new Claim(JwtRegisteredClaimNames.Sid, model.Id.ToString()),//  证件Id   
                    new Claim(ClaimTypes.Role, rolem.RoleName) //证件的角色，以后控制器上就可以直接这样写 [Authorize(Roles = "test")]
                 };

                // 2. 准备 安全密钥。就是： JwtParameter.IssuerSigningKey

                //3.准备 数字签名的安全密钥、算法和摘要。
                SigningCredentials creds = new SigningCredentials(JwtParameter.IssuerSigningKey, SecurityAlgorithms.HmacSha256);

                //4.实例化JWT得到token，
                JwtSecurityToken jst = new JwtSecurityToken(
                    issuer: JwtParameter.ValidIssuer,
                    audience: JwtParameter.ValidIssuer,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds
                );

                // 5. 以精简序列化格式将JWT安全令牌序列化为WT。拿到最终的token
                jwtStr = new JwtSecurityTokenHandler().WriteToken(jst);
                #endregion

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
        [HttpPost]
        [Authorize(Policy = "EveoneAdmin")] // 等价于 [Authorize(Roles = "admin_b")]
        public MessageModel GetUserList()
        {
            _msg.data = "你获得授权，成功进来了";
            return _msg;
        }

        /// <summary>
        /// 更新一个用户
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Policy = "EveoneAdmin")]  
        public MessageModel UpdataUser(int id, string Jsonstr)
        {
            JObject jObject = JObject.Parse(Jsonstr);
            zCustomUser zCustom = new zCustomUser();
            var props = zCustom.GetType().GetProperties();
            foreach (var item in jObject)
            {
                foreach (var prop in props)
                {
                    if (prop.Name.Equals(item.Key))
                    {
                        var s = Convert.ChangeType(item.Value, prop.PropertyType); // s需要做数据库关键词过滤
                        prop.SetValue(zCustom, s, null);
                    }
                }
            }
            //_msg.success = _userver.Updateable(k => new zCustomUser() { Name = "小的" }, k => k.Id == id); //举例
            _msg.success = _userver.Updateable(k => zCustom, k => k.Id == id);

            return _msg;
        }

        //前端直接传递Json，
        public MessageModel UpdataUser1([FromBody]zCustomUser zCustom)
        {
            //zCustom 包含Id，与需要修改的列名，有Id，不知道行不行
            _msg.success = _userver.Updateable(k => zCustom, k => k.Id == zCustom.Id);
            return _msg;
        }

        /// <summary>
        /// 测试json配置文件是否可以读取及时数据
        /// </summary>
        /// <returns></returns>
        public string pp()
        {
            return QinCommon.Common.Appsettings
                .app(new string[] { "a" }); 
        }
    }
}