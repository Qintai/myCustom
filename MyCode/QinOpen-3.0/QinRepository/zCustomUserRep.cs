using IQinRepository;
using QinEntity;
using SqlSugar;

namespace QinRepository
{
    /// <summary>
    /// 因为泛型，不是具体，批量注入。
    /// 中间类
    /// 作用：为了让autofac 把不同实体的仓储给注入进去
    /// </summary>
    public class zCustomUserRep
        : SqlSugerHelper<zCustomUser>, IzCustomUserRep 
    {
        public zCustomUserRep(ISqlSugarClient db)
            :base(db)
        {

        }
    }
}
