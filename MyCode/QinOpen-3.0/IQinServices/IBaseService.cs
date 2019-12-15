using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IQinServices
{
    /// <summary>
    /// 注意这个接口才是基础设施的根本，用于服务层的数据持久化
    /// </summary>
    /// <typeparam name="entity"></typeparam>
    public interface IBaseService<entity> where entity : class, new()
    {
        entity this[object objId] { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objId">必须指定主键特性   [Column("objId", IsPrimaryKey = true)] </param>
        /// 默认为Id
        /// <returns></returns>
        entity GetModel(object objId);

        entity GetModel(Expression<Func<entity, bool>> predicate);

        List<entity> GetList();


        /**********************************************************************************************************************/

        //Task<entity> this[int index] { get; }

        Task<entity> GetModelAsync(object Id, bool blnUseCache = false);

        Task<entity> GetModelAsync(Expression<Func<entity, bool>> predicate);

        Task<List<entity>> GetListAsync();

        Task<List<entity>> GetListByIds(params object[] lstIds);

        bool Updateable(Expression<Func<entity, object>> columns, Expression<Func<entity, bool>> expression);

        bool UpdateableByDictionary(Dictionary<string, object> dt);
    }
}
