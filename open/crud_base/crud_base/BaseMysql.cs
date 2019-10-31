using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Chloe;

namespace crud_base
{
    /// <summary>
    ///     公共的crud操作
    /// </summary>
    /// <typeparam name="entity"></typeparam>
    public abstract class BaseMysql<entity> : IBaseMysql<entity> where entity : class
    {
        protected IDbContext context;

        public BaseMysql(IDbContext _context)
        {
            context = _context;
        }

        public entity this[int Id] => GetModel(Id);

        public List<entity> GetList()
        {
            return context.Query<entity>().ToList();
        }

        public abstract entity GetModel(int Id);

        public entity GetModel(Expression<Func<entity, bool>> predicate)
        {
            return context.Query<entity>().First(predicate);
        }
    }
}