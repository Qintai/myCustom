﻿using System;
using Chloe.Annotations;

namespace crud_entity
{
    /// <summary>
    /// 按钮跟权限关联表
    /// </summary>
    public class RoleModulePermission 
    {
        public RoleModulePermission()
        {
            //this.Role = new Role();
            //this.Module = new Module();
            //this.Permission = new Permission();
        }

        [Column("Id", IsPrimaryKey = true)]
        public int Id { get; set; }

        /// <summary>
        ///获取或设置是否禁用，逻辑上的删除，非物理删除
        /// </summary>
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        public int ModuleId { get; set; }

        /// <summary>
        /// 按钮ID
        /// </summary>
        public int? PermissionId { get; set; }

        /// <summary>
        /// 创建ID
        /// </summary>
        public int? CreateId { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 修改ID
        /// </summary>
        public int? ModifyId { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        public string ModifyBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; } = DateTime.Now;

        // 下边三个实体参数，只是做传参作用，所以忽略下
        public Role Role { get; set; }

        //public Module Module { get; set; }

        //public Permission Permission { get; set; }
    }
}
