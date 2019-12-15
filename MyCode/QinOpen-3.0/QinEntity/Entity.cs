using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinEntity
{
    public class Entity
    {
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

    }
}
