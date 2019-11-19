using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QinOpen
{
    public class ViewModel
    {
        [MyRequiredAttribute(ErrorMessage ="姓名不能为空")]
        public string Name { get; set; }

        [Range(3, 21, ErrorMessage = "密码长度不够")]
        public int pwd { get; set; }
    }
}
