using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinEntity
{
    public class Menus : Entity
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        [SugarColumn(ColumnDescription = "菜单名称")]
        public string MenuName { get; set; }

        /// <summary>
        /// 菜单URL
        /// </summary>
        [SugarColumn(ColumnDescription = "菜单URL")]
        public string MenuUrl { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [SugarColumn(ColumnDescription = "是否启用")]
        public int Tag { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [SugarColumn(ColumnDescription = "父级Id")]
        public int Fid { get; set; }
    }
}
