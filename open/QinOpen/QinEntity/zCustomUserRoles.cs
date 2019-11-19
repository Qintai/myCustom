using System;
using SqlSugar;

namespace QinEntity
{
    /// <summary>
    /// 用户角色表
    /// </summary>
    public class zCustomUserRoles
    {

        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        ///获取或设置是否禁用，逻辑上的删除，非物理删除
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool? IsDeleted { get; set; }

        public int userId { get; set; }

        public int RoleId { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? addTime { get; set; }

    }

}