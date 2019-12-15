using IQinServices;
using QinEntity;
using IQinRepository;

namespace QinServices
{
    internal class zCustomUserService  : BaseService<zCustomUser>, IzCustomUserService
    {
        //public zCustomUserService(IBaseRepository<zCustomUser> _db) : base(_db)
        //{

        //}

        public zCustomUserService(IzCustomUserRep rep)
          : base(rep)
        {

        }

    }
}
