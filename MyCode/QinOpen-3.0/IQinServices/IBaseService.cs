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
        #region MyRegion
        //     entity this[object objId] { get; }
        //
        //     /// <summary>
        //     /// 
        //     /// </summary>
        //     /// <param name="objId">必须指定主键特性   [Column("objId", IsPrimaryKey = true)] </param>
        //     /// 默认为Id
        //     /// <returns></returns>
        //     entity GetModel(object objId);
        //
        //     entity GetModel(Expression<Func<entity, bool>> predicate);
        //
        //     List<entity> GetList();
        //
        //
        //     /**********************************************************************************************************************/
        //
        //     //Task<entity> this[int index] { get; }
        //
        //     Task<entity> GetModelAsync(object Id, bool blnUseCache = false);
        //
        //     Task<entity> GetModelAsync(Expression<Func<entity, bool>> predicate);
        //
        //     Task<List<entity>> GetListAsync();
        //
        //     Task<List<entity>> GetListByIds(params object[] lstIds);
        //
        //     bool Updateable(Expression<Func<entity, object>> columns, Expression<Func<entity, bool>> expression);
        //
        //     bool UpdateableByDictionary(Dictionary<string, object> dt);
        #endregion


        #region 添加操作
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="parm">cms_advlist</param>
        /// <returns></returns>
        Task<int> AddAsync(entity parm);

        int Add(entity parm);

        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="parm">List<entity></param>
        /// <returns></returns>
        Task<int> AddListAsync(List<entity> parm);

        int AddList(List<entity> parm);

        #endregion

        #region 查询操作
        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="where">Expression<Func<entity, bool>></param>
        /// <param name="order">Expression<Func<entity, object>></param>
        /// <param name="orderby">DbOrderEnum</param>
        /// <returns></returns>
        Task<List<entity>> GetListAsync(Expression<Func<entity, bool>> where, Expression<Func<entity, object>> order, string orderby);

        List<entity> GetList(Expression<Func<entity, bool>> where, Expression<Func<entity, object>> order, string orderby);

        /// <summary>
        /// 获得列表
        /// </summary>
        /// <returns></returns>
        Task<List<entity>> GetListAsync(bool Async = true);

        List<entity> GetList();

        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <param name="parm">string</param>
        /// <returns></returns>
        Task<entity> GetModelAsync(string parm);

        entity GetModel(string parm);

        /// <summary>
        /// 获得一条数据
        /// </summary>
        /// <param name="where">Expression<Func<entity, bool>></param>
        /// <returns></returns>
        Task<entity> GetModelAsync(Expression<Func<entity, bool>> where);

        entity GetModel(Expression<Func<entity, bool>> where);

        #endregion

        #region 修改操作
        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <param name="parm">entity</param>
        /// <returns></returns>
        Task<int> UpdateAsync(entity parm);

        int Update(entity parm);

        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <param name="parm">entity</param>
        /// <returns></returns>
        Task<int> UpdateAsync(List<entity> parm);

        int Update(List<entity> parm);

        /// <summary>
        /// 修改一条数据，可用作假删除
        /// </summary>
        /// <param name="columns">修改的列=Expression<Func<entity,entity>></param>
        /// <param name="where">Expression<Func<entity,bool>></param>
        /// <returns></returns>
        Task<int> UpdateAsync(Expression<Func<entity, entity>> columns, Expression<Func<entity, bool>> where);

        int Update(Expression<Func<entity, entity>> columns, Expression<Func<entity, bool>> where);
        #endregion

        #region 删除操作
        /// <summary>
        /// 删除一条或多条数据
        /// </summary>
        /// <param name="parm">string</param>
        /// <returns></returns>
        Task<int> DeleteAsync(string parm);
        int Delete(string parm);

        /// <summary>
        /// 删除一条或多条数据
        /// </summary>
        /// <param name="where">Expression<Func<entity, bool>></param>
        /// <returns></returns>
        Task<int> DeleteAsync(Expression<Func<entity, bool>> where);
        int Delete(Expression<Func<entity, bool>> where);

        #endregion

        #region 查询Count
        Task<int> CountAsync(Expression<Func<entity, bool>> where);
        int Count(Expression<Func<entity, bool>> where);
        #endregion

        #region 是否存在
        Task<bool> IsExistAsync(Expression<Func<entity, bool>> where);
        bool IsExist(Expression<Func<entity, bool>> where);
        #endregion

    }
}
