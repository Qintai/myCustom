using IQinServices;
using QinEntity;
using IQinRepository;
using System.Collections.Generic;
using AutoMapper;

namespace QinServices
{
    internal class UserService : BaseService<zCustomUser>, IUserService
    {
        /// <summary>
        /// 基础仓储
        /// </summary>
        private IzCustomUserRep _userrep;

        /// <summary>
        /// 文章仓储
        /// </summary>
        private IZArticleRep _artrep;

        private IMapper _mapper;

        /// <summary>
        /// 基础crud→ IzCustomUserRep
        /// 文章业务→ IZArticleRep
        /// </summary>
        /// <param name="userrep"></param>
        public UserService(IzCustomUserRep userrep, IZArticleRep artrep, IMapper mapper)
          : base(userrep)
        {
            _userrep = userrep;
            _artrep = artrep;
            _mapper = mapper;
        }

        public int AddUser(AddUserDTO dto)
        {
            zCustomUser model = _mapper.Map<zCustomUser>(dto);
            return base.Add(model);
        }

        /// <summary>
        /// 当前用户今天发布指定文章的访问量
        /// </summary>
        /// <returns></returns>
        public int GetUserArticlePv(int id, int uid)
        {
            ZArticle model = _artrep.GetModel(m => m.Id == id);
            zCustomUser user = base.GetModel(m => m.Id == uid);
            if (model.UserId != user.Id)
            {
                // 用户与当前关系对应不上
            }

            return 0;
        }

        /// <summary>
        /// 当前用户发布的所有文章
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserArticlePO> UserArticleList()
        {
            throw new System.NotImplementedException();
        }

    }
}
