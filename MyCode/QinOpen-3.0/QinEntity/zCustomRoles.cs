using SqlSugar;
using System;

namespace QinEntity
{
    /// <summary>
    /// 当前角色表
    /// </summary>
    public class zCustomRoles
    {

        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }


        public string Ranlk { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? addTime { get; set; } = DateTime.Now;
    }

}