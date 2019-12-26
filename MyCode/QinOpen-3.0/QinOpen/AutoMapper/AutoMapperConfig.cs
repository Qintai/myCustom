using AutoMapper;
using QinEntity;
using System;
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
            // 第一个参数是原对象，第二个是目的对象，
            //    CreateMap<zCustomUser, AddUserDTO>()
            //        //左边是 DTO                               右边是实体
            //        .ForMember(a=>a.daddTime,b=>b.MapFrom(s=>s.addTime))
            //        .ForMember(a=>a.daddTime,b=>b.MapFrom(s=>s.addTime));

            /*把dto转换成实体*/
            CreateMap<AddUserDTO, zCustomUser>()
                .ForMember(a => a.AddTime, b => b.MapFrom(s => s.daddTime)) //添加时间
                .ForMember(a => a.Name, b => b.MapFrom(s => s.dName)) //用户昵称
                .ForMember(a => a.pwd, b => b.MapFrom(s => s.dpwd))//用户密码
                .ForMember(a => a.Gender, b => b.MapFrom(s => s.dSex)); //用户性别 ，这样写会报错的，因为string 不能转int，需要下面的 ConvertUsing
            CreateMap<string, int>().ConvertUsing((y, m) =>
            {
                if (y.Equals("男"))
                    return 1;
                else if (y.Equals("女"))
                    return 2;
                else
                    throw new Exception($"{y}转成int失败，不符合业务逻辑！");
            });

            //  CreateMap<string, int>().ConvertUsing(new IntTypeConverter());
            // CreateMap<string, int>().ConvertUsing<IntTypeConverter>();
        }
    }

    //转换器
    public interface ITypeConverter<in TSource, TDestination>
    {
        /// <summary>
        /// 转换器
        /// </summary>
        /// <param name="source">源类型</param>
        /// <param name="destination">目标类型</param>
        /// <param name="context"></param>
        /// <returns></returns>
        TDestination Convert(TSource source, TDestination destination, ResolutionContext context);
    }

    /// <summary>
    /// string 转int
    /// </summary>
    public class IntTypeConverter : ITypeConverter<string, int>
    {
        public int Convert(string source, int destination, ResolutionContext context)
        {
            if (source.Equals("男"))
                return 1;
            else if (source.Equals("女"))
                return 2;
            else
                throw new Exception($"{source}转成int失败，不符合业务逻辑！");
        }
    }

    /// <summary>
    /// string 转 Datetime
    /// </summary>
    public class DateTimeTypeConverter : ITypeConverter<string, DateTime>
    {
        public DateTime Convert(string source, DateTime destination, ResolutionContext context)
        {
            return System.Convert.ToDateTime(source);
        }
    }

}
