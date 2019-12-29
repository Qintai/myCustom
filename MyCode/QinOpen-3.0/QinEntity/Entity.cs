using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinEntity
{
    /// <summary>
    /// 公共实体类型
    /// </summary>
    public class Entity
    {
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

    }
}
