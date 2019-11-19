using QinEntity;
using QinRepository;
using SqlSugar;

namespace QinServices
{
    public class zCustomUserRolesService : SqlSuger<zCustomUserRoles>
    {
        public zCustomUserRolesService(ISqlSugarClient _db) : base(_db)
        {

        }

    }
}
