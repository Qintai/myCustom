using System;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace QinEntity
{
    /// <summary>
    /// 添加用户的dto
    /// </summary>
    public class AddUserDTO
    {
        /// <summary>
        /// 昵称
        /// </summary>
        [Required(ErrorMessage ="昵称不能为空")]
        [Display(Name = "昵称名")]
        public string Name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [StringLength(160, MinimumLength = 6)]
        public string pwd { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string Emails { get; set; }


        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime addTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 性别
        /// </summary>
        [Required(ErrorMessage = "请输入性别")]
        public string Sex { get; set; }

    }
}