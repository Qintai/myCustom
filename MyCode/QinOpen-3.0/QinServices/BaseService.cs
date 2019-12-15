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

        public TEntity this[object objId] => throw new NotImplementedException();

        public List<TEntity> GetList()
        {
            return this.repository.GetList();
        }

        public Task<List<TEntity>> GetListAsync()
        {
            return this.repository.GetListAsync();
        }

        public Task<List<TEntity>> GetListByIds(params object[] lstIds)
        {
            return this.repository.GetListByIds();
        }

        public TEntity GetModel(object objId)
        {
            return this.repository.GetModel(objId);
        }

        public TEntity GetModel(Expression<Func<TEntity, bool>> predicate)
        {
            return this.repository.GetModel(predicate);
        }

        public Task<TEntity> GetModelAsync(object Id, bool blnUseCache = false)
        {
            return this.repository.GetModelAsync(Id, blnUseCache);
        }

        public Task<TEntity> GetModelAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return this.repository.GetModelAsync(predicate);
        }

        public bool Updateable(Expression<Func<TEntity, object>> columns, Expression<Func<TEntity, bool>> expression)
        {
            return this.repository.Updateable(columns, expression);
        }

        public bool UpdateableByDictionary(Dictionary<string, object> dt)
        {
            return this.repository.UpdateableByDictionary(dt);
        }
    }
}
