using Chloe;
using QinEntity;
using QinRepository;

namespace QinServices
{
    public class zUserService : ChioeRepository<zUser>, IzUserService
    {
        public zUserService(IDbContext context) : base(context)
        {
        }


    }
}
