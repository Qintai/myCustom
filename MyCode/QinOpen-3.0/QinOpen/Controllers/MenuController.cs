using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IQinServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QinCommon.Common.Helper;
using QinEntity;

namespace QinOpen.Controllers
{
    /// <summary>
    /// 关于菜单
    /// </summary>
    [Route("api/Menu/[action]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        MessageModel _msg = new MessageModel();
        IRolesService _rolesService;
        IMenusService _menuservice;

        public MenuController(IRolesService rolesService, IMenusService menuservice)
        {
            _rolesService = rolesService;
            _menuservice = menuservice;
        }

        /// <summary>
        /// 获取路由权限树
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MessageModel GetNavigationBar()
        {
            var authorization = HttpContext.Request.Headers["Authorization"];
            string jwtstr = authorization.ToString().Replace("Bearer ", "");
            TokenModelJwt token = JwtHelper.SerializeJwt(jwtstr);

            Enum.TryParse<RoleHelper.Roletype>(token.Role, out RoleHelper.Roletype roletype);
            int roleId = (int)roletype;
            Roles model = _rolesService.GetModel(a => a.Id == roleId);
            if (model != null)
            {
                var menusIds = model.Menus.Split(',');
                List<Menus> menulist = new List<Menus>();
                foreach (string item in menusIds)
                {
                    var m = _menuservice.GetModel(a => a.Id == int.Parse(item));
                    if (m != null)
                        menulist.Add(m);
                }
                List<NavigationBar> all = new List<NavigationBar>();
                all = menulist.Select(child => new NavigationBar
                {
                    id = child.Id,
                    name = child.MenuName,
                    pid = child.Fid,
                    order = 1,
                    path=child.MenuUrl,
                    meta = new NavigationBarMeta
                    {
                        requireAuth = true,
                        title = child.MenuName,
                        NoTabPage = true
                    }
                }).ToList();

                //这时，所有角色所对应 菜单列表已经出来
                NavigationBar rootRoot = new NavigationBar()
                {
                    id = 0,
                    pid = 0,
                    order = 0,
                    name = "根节点",
                    path = "",
                    iconCls = "",
                    meta = new NavigationBarMeta(),
                };
                Recursion(all, rootRoot);
               _msg.Response = rootRoot;
            }

            _msg.Success = true;
            _msg.Message = "路由节点获取成功！";
            return _msg;
        }

        /// <summary>
        /// 递归求出下一级菜单
        /// </summary>
        /// <param name="menulist"></param>
        /// <param name="rootRoot"></param>
        [NonAction]
        public void Recursion(List<NavigationBar> menulist, NavigationBar rootRoot)
        {
            var subItems = menulist.Where(ee => ee.pid == rootRoot.id).ToList();
            if (subItems.Count()>0)
            {
                rootRoot.children = new List<NavigationBar>();
                rootRoot.children.AddRange(subItems);
            }
            else
                rootRoot.children = null;
            foreach (var item in subItems)
            {
                Recursion(menulist, item);
            }
        }

    }

}
