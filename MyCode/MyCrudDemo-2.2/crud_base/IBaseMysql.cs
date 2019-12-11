using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace crud_base
{
    /// <summary>
    ///     crud 基本
    /// </summary>
    /// <typeparam name="entity"></typeparam>
    public interface IBaseMysql<entity>
    {
        entity this[int index] { get; }
        entity GetModel(int Id);

        entity GetModel(Expression<Func<entity, bool>> predicate);

        List<entity> GetList();
    }
}