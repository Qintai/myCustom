using System;
using Chloe.Annotations;

namespace crud_entity
{
    public class zunderlying
    {
        /// <summary>
        /// GUID
        /// </summary>
        [Column("Id", IsPrimaryKey = true)]
        public Guid GuId { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public Gender? pwd { get; set; }

        /// <summary>
        /// 权限Id
        /// </summary>
        private int RoleId;

        /// <summary>
        /// 权限
        /// </summary>
        [NotMapped]
        public string Role
        {
            get
            {
                Enum.TryParse<Permissions>(this.RoleId.ToString(), out Permissions f);
                return f.ToString(); 
            }
            set
            {
                Enum.TryParse<Permissions>(value.ToString(), out Permissions f);
                this.RoleId = (int)f;
            }
        }
    }
}
