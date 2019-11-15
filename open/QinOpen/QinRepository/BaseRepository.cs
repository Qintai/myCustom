using QinIRepository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace QinRepository
{
    public class BaseRepository<entity> : IBaseRepository<entity>
    {
        public entity this[int index] => throw new NotImplementedException();

        public List<entity> GetList()
        {
            throw new NotImplementedException();
        }

        public Task<List<entity>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        public entity GetModel(int Id)
        {
            throw new NotImplementedException();
        }

        public entity GetModel(Expression<Func<entity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<entity> GetModelAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<entity> GetModelAsync(Expression<Func<entity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

    }
}
