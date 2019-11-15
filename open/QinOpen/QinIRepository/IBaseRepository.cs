using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QinIRepository
{
    public interface IBaseRepository<entity>
    {
        /**************/
        entity this[int index] { get; }

        entity GetModel(int Id);

        entity GetModel(Expression<Func<entity, bool>> predicate);

        List<entity> GetList();
        /**************/

        //Task<entity> this[int index] { get; }

        Task<entity> GetModelAsync(int Id);

        Task<entity> GetModelAsync(Expression<Func<entity, bool>> predicate);

        Task<List<entity>> GetListAsync();
    }
}
