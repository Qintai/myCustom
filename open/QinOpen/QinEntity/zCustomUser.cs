using System;
using SqlSugar;

namespace QinEntity
{
    /// <summary>
    /// 当前用户表
    /// </summary>
    public class zCustomUser
    {
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "主键ID")]
        public int Id { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [SugarColumn(ColumnDescription = "名称")]
        public string Name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string pwd { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "性别")]
        public int? Gender { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [SugarColumn(ColumnDescription = "添加时间")]
        public DateTime? addTime { get; set; }

        /// <summary>
        /// 业务字段：性别
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string MyProperty
        {
            get
            {
                return Gender.Value == 1 ? "男" : "女";
            }
        }

        
    }
}