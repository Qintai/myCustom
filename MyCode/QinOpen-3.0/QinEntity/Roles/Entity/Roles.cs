using SqlSugar;
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
        /// 角色名称
        /// </summary>
        [SugarColumn(ColumnDescription = "名称")]
        public string Name { get; set; }

        /// <summary>
        /// 角色可以访问的页面，多个菜单Id,逗号分隔
        /// </summary>
        [SugarColumn(ColumnDescription = "菜单")]
        public string Menus { get; set; }

        /// <summary>
        /// 角色的创建者
        /// </summary>
        [SugarColumn(ColumnDescription = "创建者")]
        public int CreateId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnDescription = "创建者")]
        public DateTime AddTime { get; set; }
    }
}
