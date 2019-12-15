using IQinRepository;
using IQinServices;
using QinEntity;
using SqlSugar;

namespace QinServices
{
    /// <summary>
    /// 因为 BaseService 实现了基层接口，不用再次实现 zCustomUserRolesService的父级方法
    /// </summary>
    internal class zCustomUserRolesService : BaseService<zCustomUserRoles>
        , IzCustomUserRolesService
    {
        /// <summary>
        ///  依赖注入，把仓库管理员给过来
        ///  IBaseRepository 。autofac ，泛型注入不了
        /// </summary>
        /// <param name="rep">数据持久层的具体实现者</param>
        //public zCustomUserRolesService(IBaseRepository<zCustomUserRoles> rep)
        //    : base(rep)
        //{ }

        
        
        public zCustomUserRolesService(IzCustomUserRolesRep rep)
            : base(rep)
        {

        }

    }
}
