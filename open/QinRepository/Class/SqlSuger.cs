using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace QinRepository
{
    public class SqlSuger<entity> : IBaseRepository<entity>
    {
        private readonly ISqlSugarClient _db;

        public  SqlSuger(ISqlSugarClient db)
        {
            _db = db;
        }

        public entity this[object objId] => GetModel(objId);

        public List<entity> GetList()
        {
            return _db.Queryable<entity>().ToList();
        }

        public Task<List<entity>> GetListAsync()
        {
            return _db.Queryable<entity>().ToListAsync();
        }

        public async Task<entity> GetModelAsync(object objId, bool blnUseCache = false)
        {
            return await _db.Queryable<entity>().WithCacheIF(blnUseCache).In(objId).SingleAsync();
        }
        public entity GetModel(object objId)
        {
            return _db.Queryable<entity>().In(objId).Single();
        }

        public Task<entity> GetModelAsync(Expression<Func<entity, bool>> predicate)
        {
            return _db.Queryable<entity>().Where(predicate).FirstAsync();
        }

        public entity GetModel(Expression<Func<entity, bool>> predicate)
        {
            return _db.Queryable<entity>().Where(predicate).First();
        }

        /// <summary>
        /// 功能描述:根据ID查询数据
        /// </summary>
        /// <param name="lstIds">id列表（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <returns>数据实体列表</returns>
        public async Task<List<entity>> GetListByIds(params object[] lstIds)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().In(lstIds).ToList());
            return await _db.Queryable<entity>().In(lstIds).ToListAsync();
        }


    }
}


