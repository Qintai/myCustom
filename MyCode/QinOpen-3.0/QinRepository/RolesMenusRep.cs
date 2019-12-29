using IQinRepository;
using QinEntity;
using SqlSugar;

namespace QinRepository
{
    public class RolesMenusRep : SqlSugerHelper<Menus>, IRolesMenusRep
    {
        public RolesMenusRep(ISqlSugarClient db)
            : base(db)
        {

        }
    }
}
