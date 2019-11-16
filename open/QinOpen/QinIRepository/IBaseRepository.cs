﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QinIRepository
{
    public interface IBaseRepository<entity>
    {
        /**************/
        entity this[object objId] { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objId">必须指定主键特性   [Column("objId", IsPrimaryKey = true)] </param>
        /// 默认为Id
        /// <returns></returns>
        entity GetModel(object objId);

        entity GetModel(Expression<Func<entity, bool>> predicate);

        List<entity> GetList();


        /**********************************************************************************************************************/

        //Task<entity> this[int index] { get; }

        Task<entity> GetModelAsync(object Id, bool blnUseCache = false);

        Task<entity> GetModelAsync(Expression<Func<entity, bool>> predicate);

        Task<List<entity>> GetListAsync();

        Task<List<entity>> GetListByIds(params object[] lstIds);


    }
}
