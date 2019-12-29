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
        public string MenuName { get; set; }

        /// <summary>
        /// 菜单URL
        /// </summary>
        public string MenuUrl { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int Fid { get; set; }
    }
}
