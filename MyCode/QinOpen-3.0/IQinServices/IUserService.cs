using QinEntity;
using IQinRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace IQinServices
{
    /// <summary>
    /// 关于当前用户的一系列处理逻辑
    /// </summary>
    public interface IUserService : IBaseRepository<zCustomUser>
    {
        /// <summary>
        /// 当前用户发布的所有文章
        /// </summary>
        /// <returns></returns>
         IEnumerable<UserArticlePO> UserArticleList();


        /// <summary>
        /// 当前用户今天发布指定文章的访问量
        /// </summary>
        /// <returns></returns>
         int GetUserArticlePv(int id,int uid);

        /// <summary>
        /// 添加一个系统用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
         int AddUser(AddUserDTO dto);

    }
}
