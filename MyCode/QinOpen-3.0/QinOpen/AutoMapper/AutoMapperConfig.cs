using AutoMapper;
using QinEntity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QinOpen
{
    /// <summary>
    /// AutoMap的配置
    /// </summary>
    public class AutoMapperConfig
    {
        internal static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CustomProfile());
            });
        }
    }

    public class CustomProfile : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public CustomProfile()
        {
            CreateMap<zCustomUser, AddUserDTO>();
            CreateMap<AddUserDTO, zCustomUser>();
        }
    }

}
