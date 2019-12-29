using System;
using System.Collections.Generic;
using System.Text;

namespace QinEntity
{
    /// <summary>
    /// 角色表
    /// </summary>
    public class Roles : Entity
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public int Name { get; set; }

        /// <summary>
        /// 角色可以访问的页面，多个菜单Id,逗号分隔
        /// </summary>
        public string Menus { get; set; }

        /// <summary>
        /// 角色的创建者
        /// </summary>
        public int CreateId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime AddTime { get; set; }
    }
}
