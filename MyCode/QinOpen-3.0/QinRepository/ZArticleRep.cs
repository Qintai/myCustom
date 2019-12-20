using IQinRepository;
using QinEntity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace QinRepository
{
    public class ZArticleRep : SqlSugerHelper<ZArticle>, IZArticleRep
    {
        public ZArticleRep(ISqlSugarClient db)
            : base(db)
        {

        }
    }
}
