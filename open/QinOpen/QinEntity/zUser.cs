using System;
using Chloe.Annotations;

namespace QinEntity
{
    [Table("zUser")]
    public class zUser
    {
        /// <summary>
        /// 名字
        /// </summary>
        [Column("Id", IsPrimaryKey = true)]
        public int Id { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Gender? Gender { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? CityId { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? addTime { get; set; }

        //  [NotMapped]
        //   public string NotMappedProperty { get; set; }
        /// <summary>
        /// 映射的实体类
        /// </summary>
        //  public string NotMapped { get; set; }
    }

    public enum Gender
    {
        Man = 1,
        Woman
    }
}