using IQinRepository;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace QinRepository
{
    public class SqlSugerHelper<entity> : IBaseRepository<entity> where entity : QinEntity.Entity, new()
    {
        private readonly ISqlSugarClient _db;

        public SqlSugerHelper(ISqlSugarClient db)
        {
            _db = db;
        }

        public int Add(entity parm)
        {
            return _db.Insertable<entity>(parm).ExecuteCommand();
        }

        public async Task<int> AddAsync(entity parm)
        {
            return await _db.Insertable<entity>(parm).ExecuteCommandAsync();
        }

        public async Task<int> AddListAsync(List<entity> parm)
        {
            return await _db.Insertable<entity>(parm).ExecuteCommandAsync();
        }

        public int Count(Expression<Func<entity, bool>> where)
        {
            return _db.Queryable<entity>().Count(where);
        }

        public async Task<int> CountAsync(Expression<Func<entity, bool>> where)
        {
            return _db.Queryable<entity>().Count(where);
        }

        public int Delete(Expression<Func<entity, bool>> where)
        {
            return _db.Deleteable<entity>().Where(where).ExecuteCommand();
        }

        public Task<int> DeleteAsync(string parm)
        {
            var list = parm.Split(',').ToList();
            return _db.Deleteable<entity>().In(list.ToArray()).ExecuteCommandAsync();
        }

        public int Delete(string parm)
        {
            var list = parm.Split(',').ToList();
            return _db.Deleteable<entity>().In(list.ToArray()).ExecuteCommand();
        }

        public async Task<int> DeleteAsync(Expression<Func<entity, bool>> where)
        {
            return await _db.Deleteable<entity>().Where(where).ExecuteCommandAsync();
        }

        public List<entity> GetList(Expression<Func<entity, bool>> where, Expression<Func<entity, object>> order, string orderby)
        {
            SqlSugar.OrderByType orderByType = SqlSugar.OrderByType.Asc;
            if (orderby.Equals("Asc"))
                orderByType = SqlSugar.OrderByType.Asc;
            else if (orderby.Equals("Desc"))
                orderByType = SqlSugar.OrderByType.Desc;

            var query = _db.Queryable<entity>().Where(where).OrderBy(order, orderByType);
            return query.ToList();
        }

        public List<entity> GetList()
        {
            return _db.Queryable<entity>().ToList();
        }

        public async Task<List<entity>> GetListAsync(Expression<Func<entity, bool>> where, Expression<Func<entity, object>> order, string orderby)
        {
            SqlSugar.OrderByType orderByType = SqlSugar.OrderByType.Asc;
            if (orderby.Equals("Asc"))
                orderByType = SqlSugar.OrderByType.Asc;
            else if (orderby.Equals("Desc"))
                orderByType = SqlSugar.OrderByType.Desc;

            var query = _db.Queryable<entity>().Where(where).OrderBy(order, orderByType);
            return await query.ToListAsync();
        }

        public async Task<List<entity>> GetListAsync(bool Async = true)
        {
            return await _db.Queryable<entity>().ToListAsync();
        }

        public entity GetModel(string parm)
        {
            return _db.Queryable<entity>().Where(parm).First() ?? new entity() { };
        }

        public entity GetModel(Expression<Func<entity, bool>> where)
        {
            return _db.Queryable<entity>().Where(where).First() ?? new entity() { };
        }

        public async Task<entity> GetModelAsync(string parm)
        {
            return await _db.Queryable<entity>().Where(parm).FirstAsync() ?? new entity() { };
        }

        public async Task<entity> GetModelAsync(Expression<Func<entity, bool>> where)
        {
            return await _db.Queryable<entity>().Where(where).FirstAsync() ?? new entity() { };
        }

        public async Task<bool> IsExistAsync(Expression<Func<entity, bool>> where)
        {
            var dbres = await _db.Queryable<entity>().AnyAsync(where);
            return dbres;
        }

        public bool IsExist(Expression<Func<entity, bool>> where)
        {
            var dbres = _db.Queryable<entity>().Any(where);
            return dbres;
        }

        public int Update(entity parm)
        {
            return _db.Updateable<entity>(parm).ExecuteCommand();
        }

        public int Update(List<entity> parm)
        {
            return _db.Updateable<entity>(parm).ExecuteCommand();
        }

        public int Update(Expression<Func<entity, entity>> columns, Expression<Func<entity, bool>> where)
        {
            return _db.Updateable<entity>().SetColumns(columns).Where(where).ExecuteCommand();
        }

        public async Task<int> UpdateAsync(entity parm)
        {
            return await _db.Updateable<entity>(parm).ExecuteCommandAsync();
        }

        public Task<int> UpdateAsync(List<entity> parm)
        {
            return _db.Updateable<entity>(parm).ExecuteCommandAsync();
        }

        public async Task<int> UpdateAsync(Expression<Func<entity, entity>> columns, Expression<Func<entity, bool>> where)
        {
            return await _db.Updateable<entity>().SetColumns(columns).Where(where).ExecuteCommandAsync();
        }

        public int AddList(List<entity> parm)
        {
            return _db.Insertable<entity>(parm).ExecuteCommand();
        }


        #region MyRegion
        //     public entity this[object objId] => GetModel(objId);
        //
        //     public List<entity> GetList()
        //     {
        //         return _db.Queryable<entity>().ToList();
        //     }
        //
        //     public Task<List<entity>> GetListAsync()
        //     {
        //         return _db.Queryable<entity>().ToListAsync();
        //     }
        //
        //     public async Task<entity> GetModelAsync(object objId, bool blnUseCache = false)
        //     {
        //         return await _db.Queryable<entity>().WithCacheIF(blnUseCache).In(objId).SingleAsync();
        //     }
        //     public entity GetModel(object objId)
        //     {
        //         return _db.Queryable<entity>().In(objId).Single();
        //     }
        //
        //     public Task<entity> GetModelAsync(Expression<Func<entity, bool>> predicate)
        //     {
        //         return _db.Queryable<entity>().Where(predicate).FirstAsync();
        //     }
        //
        //     public entity GetModel(Expression<Func<entity, bool>> predicate)
        //     {
        //         return _db.Queryable<entity>().Where(predicate).First();
        //     }
        //
        //     /// <summary>
        //     /// 功能描述:根据ID查询数据
        //     /// </summary>
        //     /// <param name="lstIds">id列表（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        //     /// <returns>数据实体列表</returns>
        //     public async Task<List<entity>> GetListByIds(params object[] lstIds)
        //     {
        //         //return await Task.Run(() => _db.Queryable<TEntity>().In(lstIds).ToList());
        //         return await _db.Queryable<entity>().In(lstIds).ToListAsync();
        //     }
        //
        //     /// <summary>
        //     /// http://www.codeisbug.com/Doc/8/1129
        //     /// </summary>
        //     /// <param name="columns">需要更新的列</param>
        //     /// <param name="expression">查找条件</param>
        //     public bool Updateable(Expression<Func<entity, object>> columns, Expression<Func<entity, bool>> expression)
        //     {
        //         return _db.Updateable<entity>().UpdateColumns(columns).Where(expression).ExecuteCommand() > 0;
        //     }
        //
        //
        //     public bool UpdateableByDictionary(Dictionary<string, object> dt)
        //     {
        //         //var dt = new Dictionary<string, object>();
        //         //dt.Add("id", 1);
        //         //dt.Add("name", "1");
        //         return _db.Updateable(dt).AS(nameof(entity)).ExecuteCommand() > 0;
        //     }
        #endregion


    }
}


