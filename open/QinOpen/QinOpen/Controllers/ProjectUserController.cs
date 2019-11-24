﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QinCommon.Common.Helper;
using QinEntity;
using QinOpen.AuthHelper;
using QinServices;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

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
        zCustomUserService _userver;
        zCustomUserRolesService _roleserver;

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
            _msg.data = new List<zUser>();
            return _msg;
        }

    }
}