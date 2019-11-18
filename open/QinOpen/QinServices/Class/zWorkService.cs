using Chloe;
using QinEntity;
using QinRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinServices
{
    public class zWorkService : ChioeRepository<zWork>
    {
        public zWorkService(IDbContext db) : base(db)
        {

        }

    }
}
