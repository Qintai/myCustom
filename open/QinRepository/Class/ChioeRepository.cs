using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Chloe;


namespace QinRepository
{
    public class ChioeRepository<entity> : IBaseRepository<entity>
    {
        protected IDbContext _context;

        public ChioeRepository(IDbContext context)
        {
            _context = context;
        }

        public List<entity> GetList()
        {
            return _context.Query<entity>().ToList();
        }

        public entity GetModel(Expression<Func<entity, bool>> predicate)
        {
            return _context.Query<entity>().First(predicate);
        }

        public entity this[object objId] =>GetModel(objId);

        /// 
        /// </summary>
        /// <param name="objId">必须指定主键特性   [Column("objId", IsPrimaryKey = true)] </param>
        /// 默认为Id
        /// <returns></returns>
        public entity GetModel(object objId)
        {
            // return _context.Query<entity>().First(a => a.Id == objId);

            throw new NotImplementedException();
        }

        public Task<List<entity>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<entity> GetModelAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<entity> GetModelAsync(object Id, bool blnUseCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<entity> GetModelAsync(Expression<Func<entity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 功能描述:根据ID查询数据
        /// </summary>
        /// <param name="lstIds">id列表（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <returns>数据实体列表</returns>
        public Task<List<entity>> GetListByIds(params object[] lstIds)
        {
            throw new NotImplementedException();
        }
    }
}
