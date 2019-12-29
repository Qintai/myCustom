using IQinRepository;
using QinEntity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace QinRepository
{
    public class RolesRep : SqlSugerHelper<Roles>, IRolesRep
    {
        public RolesRep(ISqlSugarClient db)
            : base(db)
        {

        }
    }
}
