using Chloe;
using QinEntity;
using QinIServices;
using QinRepository;
using System;

namespace QinServices
{
    public class zUserService : ChioeRepository<zUser>, IzUserService
    {
        public zUserService(IDbContext context) : base(context)
        {
        }


    }
}
