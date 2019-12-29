using IQinServices;
using QinEntity;
using IQinRepository;
using System.Collections.Generic;
using AutoMapper;

namespace QinServices
{
    internal class RolesService : BaseService<Roles>, IRolesService
    {
        /// <summary>
        /// 基础crud→ IzCustomUserRep
        /// 文章业务→ IZArticleRep
        /// </summary>
        /// <param name="userrep"></param>
        public RolesService(IRolesRep rolesrep)
          : base(rolesrep)
        {
 
        }

    }
}
