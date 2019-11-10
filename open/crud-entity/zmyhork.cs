using System;
using Chloe.Annotations;

namespace crud_entity
{
    [Table("zmyhork")]
    public class zmyhork
    {
        [Column("Id", IsPrimaryKey = true)]
        public int Id { get; set; }

        public DateTime? addtime { get; set; }

        public string mtime { get; set; }

        public string num { get; set; }
    }
}