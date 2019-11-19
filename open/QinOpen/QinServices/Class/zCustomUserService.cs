using QinEntity;
using QinRepository;
using SqlSugar;

namespace QinServices
{
    public class zCustomUserService : SqlSuger<zCustomUser>
    {
        public zCustomUserService(ISqlSugarClient _db) : base(_db)
        {

        }

    }
}
