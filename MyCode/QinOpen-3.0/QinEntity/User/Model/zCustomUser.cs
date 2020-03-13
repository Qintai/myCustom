using System;
using SqlSugar;

namespace QinEntity
{
    /// <summary>
    /// 当前用户表
    /// </summary>
    public class zCustomUser : Entity
    {
        /// <summary>
        /// 昵称
        /// </summary>
        [SugarColumn(ColumnDescription = "名称",ColumnDataType = "nvarchar", Length = 50)]
        public string Name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [SugarColumn(ColumnDescription = "密码", ColumnDataType = "nvarchar", Length = 50)]
        public string pwd { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "性别", ColumnDataType = "int", Length = 50)]
        public int? Gender { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "性别", ColumnDataType = "int", Length = 11)]
        public int? state { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [SugarColumn(ColumnDescription = "添加时间")]
        public DateTime? AddTime { get; set; }

        /// <summary>
        /// 业务字段：性别
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string GenderStr
        {
            get
            {
                return Gender.Value == 1 ? "男" : "女";
            }
        }


    }
}