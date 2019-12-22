using System;
using System.ComponentModel.DataAnnotations;
using QinCommon.Common.Helper;
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
        [Required(ErrorMessage = "昵称不能为空")]
        [Display(Name = "昵称名")]
        public string dName { get; set; }

        private string _dpwd;
        /// <summary>
        /// 密码
        /// </summary>
        [StringLength(160, MinimumLength = 6)]
        [Required(ErrorMessage = "请输入密码")]
        public string dpwd { get => _dpwd; 
            set 
            { 
                _dpwd = MD5Helper.MD5Encrypt32(value); 
            }
        }

        /// <summary>
        /// 邮箱，可能别的表
        /// </summary>
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        [Required(ErrorMessage = "请输入邮箱")]
        public string dEmails { get; set; }


        /// <summary>
        /// 添加时间
        /// </summary>
        public string daddTime { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Required(ErrorMessage = "请输入性别")]
        public string dSex { get; set; }

    }
}