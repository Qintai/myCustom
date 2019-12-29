using System;
using SqlSugar;

namespace QinEntity
{
    /// <summary>
    /// 用户角色表
    /// </summary>
    public class UserRoles :Entity
    {
        /// <summary>
        ///获取或设置是否禁用，逻辑上的删除，非物理删除
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [SugarColumn]
        public int userId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        [SugarColumn]
        public int RoleId { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [SugarColumn]
        public DateTime? addTime { get; set; }

        /// <summary>
        /// 业务字段 
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string RoleName
        {
            get
            {
                Enum.TryParse<RoleHelper.Roletype>(RoleId.ToString(), out RoleHelper.Roletype roletype);
                return roletype.ToString(); ;
            }
        }
    }

    /// <summary>
    /// 业务的角色都写在这里
    /// 这里的角色和数据库的角色保持一致
    /// </summary>
    public class RoleHelper
    {
        public enum Roletype : int
        {
            admin_a = 0, //数据库库中存int值
            admin_b = 1,
            admin_c = 2,
        }

        /// <summary>
        /// 角色组
        /// </summary>
        public const string EveoneAdmin = "EveoneAdmin";

    }
}