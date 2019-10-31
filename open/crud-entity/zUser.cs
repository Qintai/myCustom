using System;
using Chloe.Annotations;

namespace crud_entity
{
    [Table("zUser")]
    public class zUser
    {
        [Column("Id", IsPrimaryKey = true)] public int Id { get; set; }
        public string Name { get; set; }
        public Gender? Gender { get; set; }
        public int? Age { get; set; }
        public int? CityId { get; set; }
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