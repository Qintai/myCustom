using Blog.Core.Model;
using IQinRepository;
using IQinServices;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace QinServices
{
    /// <summary>
    /// 让仓库管理员去实现这些 IBaseService 服务层数据持久化的方法
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    internal class BaseService<TEntity> 
        : IBaseService<TEntity> where TEntity : QinEntity.Entity, new()
    {
        /// <summary>
        /// 仓库管理员。数据持久化的具体实施者
        /// </summary>
        private IBaseRepository<TEntity> repository;

        public BaseService(IBaseRepository<TEntity> _repository)
        {
           this.repository = _repository;
        }

        public int Add(TEntity parm)
        {
           return this.repository.Add(parm);
        }

        public Task<int> AddAsync(TEntity parm)
        {
            return this.repository.AddAsync(parm);
        }

        public int AddList(List<TEntity> parm)
        {
            return this.repository.AddList(parm);
        }

        public Task<int> AddListAsync(List<TEntity> parm)
        {
            return this.repository.AddListAsync(parm);
        }

        public int Count(Expression<Func<TEntity, bool>> where)
        {
            return this.repository.Count(where);
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> where)
        {
            return this.repository.CountAsync(where);
        }

        public int Delete(Expression<Func<TEntity, bool>> where)
        {
            return this.repository.Delete(where);
        }

        public Task<int> DeleteAsync(string parm)
        {
            return this.repository.DeleteAsync(parm);
        }

        public int Delete(string parm)
        {
            return this.repository.Delete(parm);
        }

        public Task<int> DeleteAsync(Expression<Func<TEntity, bool>> where)
        {
            return this.repository.DeleteAsync(where);
        }

        public List<TEntity> GetList(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> order, string orderby)
        {
            return this.repository.GetList(where, order, orderby);
        }

        public List<TEntity> GetList()
        {
            return this.repository.GetList();
        }

        public Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> order, string orderby)
        {
            return this.repository.GetListAsync(where,order,orderby);
        }

        public Task<List<TEntity>> GetListAsync(bool Async = true)
        {
            return this.repository.GetListAsync();
        }

        public TEntity GetModel(string parm)
        {
            return this.repository.GetModel(parm);
        }

        public TEntity GetModel(Expression<Func<TEntity, bool>> where)
        {
            return this.repository.GetModel(where);
        }

        public Task<TEntity> GetModelAsync(string parm)
        {
            return this.repository.GetModelAsync(parm);
        }

        public Task<TEntity> GetModelAsync(Expression<Func<TEntity, bool>> where)
        {
            return this.repository.GetModelAsync(where);
        }

        public Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> where)
        {
            return this.repository.IsExistAsync(where);
        }

        public bool IsExist(Expression<Func<TEntity, bool>> where)
        {
            return this.repository.IsExist(where);
        }

        public int Update(TEntity parm)
        {
            return this.repository.Update(parm);
        }

        public int Update(List<TEntity> parm)
        {
            return this.repository.Update(parm);
        }

        public int Update(Expression<Func<TEntity, TEntity>> columns, Expression<Func<TEntity, bool>> where)
        {
            return this.repository.Update(columns, where);
        }

        public Task<int> UpdateAsync(TEntity parm)
        {
            return this.repository.UpdateAsync(parm);
        }

        public Task<int> UpdateAsync(List<TEntity> parm)
        {
            return this.repository.UpdateAsync(parm);
        }

        public Task<int> UpdateAsync(Expression<Func<TEntity, TEntity>> columns, Expression<Func<TEntity, bool>> where)
        {
            return this.repository.UpdateAsync(columns, where);
        }

        public PageModel<TEntity> QueryPage(Expression<Func<TEntity, bool>> whereExpression, int intPageIndex = 1, int intPageSize = 20, string strOrderByFileds = null)
        {
            return this.repository.QueryPage( whereExpression, intPageIndex, intPageSize, strOrderByFileds);
        }

        #region MyRegion

        //  public TEntity this[object objId] => throw new NotImplementedException();
        //
        //  public List<TEntity> GetList()
        //  {
        //      return this.repository.GetList();
        //  }
        //
        //  public Task<List<TEntity>> GetListAsync()
        //  {
        //      return this.repository.GetListAsync();
        //  }
        //
        //  public Task<List<TEntity>> GetListByIds(params object[] lstIds)
        //  {
        //      return this.repository.GetListByIds();
        //  }
        //
        //  public TEntity GetModel(object objId)
        //  {
        //      return this.repository.GetModel(objId);
        //  }
        //
        //  public TEntity GetModel(Expression<Func<TEntity, bool>> predicate)
        //  {
        //      return this.repository.GetModel(predicate);
        //  }
        //
        //  public Task<TEntity> GetModelAsync(object Id, bool blnUseCache = false)
        //  {
        //      return this.repository.GetModelAsync(Id, blnUseCache);
        //  }
        //
        //  public Task<TEntity> GetModelAsync(Expression<Func<TEntity, bool>> predicate)
        //  {
        //      return this.repository.GetModelAsync(predicate);
        //  }
        //
        //  public bool Updateable(Expression<Func<TEntity, object>> columns, Expression<Func<TEntity, bool>> expression)
        //  {
        //      return this.repository.Updateable(columns, expression);
        //  }
        //
        //  public bool UpdateableByDictionary(Dictionary<string, object> dt)
        //  {
        //      return this.repository.UpdateableByDictionary(dt);
        //  }
        #endregion


    }
}
