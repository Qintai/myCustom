using IQinServices;
using QinEntity;
using IQinRepository;
using System.Collections.Generic;
using AutoMapper;

namespace QinServices
{
    internal class MenusService : BaseService<Menus>, IMenusService
    {
        public MenusService(IRolesMenusRep rolesrep)
          : base(rolesrep)
        {
 
        }

    }
}
