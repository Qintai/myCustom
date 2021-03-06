﻿using AutoMapper;
using IQinServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QinCommon.Common.Helper;
using QinEntity;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace QinOpen.Controllers
{
    /// <summary>
    /// 用户操作~resultful风格
    /// </summary>
    [ApiController]
    [Route("api/user")]
    [Produces("application/json")]
    //[EnableCors("LimitRequests")]
    public class ProjectUserController : Controller
    {
        MessageModel _msg = new MessageModel();
        IUserService _userver;
        IUserRolesService _roleserver;
        IMapper _mapper;

        public ProjectUserController(
            MessageModel msg,
            IUserService userver,
            IUserRolesService roleserver,
            IMapper mapper
            )
        {
            //_msg = msg;
            _userver = userver;
            _roleserver = roleserver;
            _mapper = mapper;
        }

        /// <summary>
        /// 登陆接口
        /// </summary>
        /// <param name="Name">用户名</param>
        /// <param name="pass">密码</param>
        /// <returns></returns>
        [HttpGet("Login")]
        public async Task<MessageModel> Login(string Name, string pass)
        {
            var model = await _userver.GetModelAsync(m => m.Name.Equals(Name));
            if (model == null)
                return MessageModel.Fail(ref _msg, "没有找到用户！");

            if (model.pwd != MD5Helper.MD5Encrypt32(pass))
                return MessageModel.Fail(ref _msg, "用户密码输入错误！");

            var rolem = await _roleserver.GetModelAsync(a => a.userId == model.Id);
            if (rolem != null)
            {
                TokenModelJwt tokenModel = new TokenModelJwt
                {
                    Name = model.Name,
                    Uid = model.Id,
                    Role = rolem.RoleName
                };
                _msg.Response = JwtHelper.IssueJwt(tokenModel);
            }
            return MessageModel.Ok(ref _msg, "请求成功！");
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="page">分页条数</param>
        /// <param name="key">搜索条件</param>
        /// <returns></returns>
        [HttpGet("get")]
        [Authorize(Policy = RoleHelper.EveoneAdmin)]   // 等价于 [Authorize(Roles = "admin_b")]
        public MessageModel GetUserList(int page = 1, string key = "")
        {
            _msg.Response = _userver.QueryPage(a=>true,page,20,"addtime");
            return MessageModel.Ok(ref _msg, "请求成功！");
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="type">添加/修改</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = RoleHelper.EveoneAdmin)]
        public MessageModel AddUser(AddUserDTO dto, string type)
        {
            _msg.Success = _userver.AddUser(dto) > 0;
            return _msg;
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <returns></returns>
        [HttpPut("UpdataUser")]
        [Authorize(Policy = RoleHelper.EveoneAdmin)]
        public MessageModel UpdataUser(int id, AddUserDTO dto)
        {
            // zCustomUser zCustom1 = new zCustomUser() { Name = "小名" };
            zCustomUser model = _mapper.Map<zCustomUser>(dto);
            Expression<Func<zCustomUser, zCustomUser>> column = p => model;
            Expression<Func<zCustomUser, bool>> where = k => k.Id == id;
            _msg.Success = _userver.Update(column, where) > 0;
            return _msg;
        }

        /// <summary>
        /// 获取当前JWT的信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetJwt")]
        [Authorize(Policy = RoleHelper.EveoneAdmin)]
        public MessageModel GetJwtCurrentInfo()
        {
            var authorization = HttpContext.Request.Headers["Authorization"];
            string jwtstr = authorization.ToString().Replace("Bearer ", "");
            TokenModelJwt token = JwtHelper.SerializeJwt(jwtstr);
            _msg.Response = token;
            return _msg;
        }


        [HttpGet("GetUserByToken")]
        public MessageModel GetUserByToken(string token) 
        {
            if (!string.IsNullOrEmpty(token))
            {
                var tokenModel = JwtHelper.SerializeJwt(token);
                if (tokenModel != null && tokenModel.Uid > 0)
                {
                    var userinfo =  _userver.GetModel(p=>p.Id==Convert.ToInt32(tokenModel.Uid));
                    if (userinfo != null)
                    {
                        _msg.Response = userinfo;
                        _msg.Success = true;
                        _msg.Message = "获取用户信息成功";
                    }
                }
            }
            return _msg;
        }
    }
}